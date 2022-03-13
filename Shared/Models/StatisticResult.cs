using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Models
{
    public class StatisticResult
    {
        public StatisticStrategy StatisticBy { get; set; }

        public IDictionary<string, StatisticResultItem> Details { get; set; }

        public double HighestIncome { get; set; }

        public double LowestIncome { get; set; }

        public StatisticDateResult HighestDate { get; set; }

        public StatisticDateResult LowestDate { get; set; }

        protected StatisticResult(StatisticStrategy strategy,
            SortedDictionary<StatisticDateResult, StatisticResultItem> items)
        {
            StatisticBy = strategy;
            HighestIncome = items.Max(item => item.Value.Income);
            LowestIncome = items.Min(item => item.Value.Income);
            HighestDate = items.First(item => item.Value.Income == HighestIncome).Key;
            LowestDate = items.First(item => item.Value.Income == LowestIncome).Key;
            if (StatisticBy == StatisticStrategy.ByDay)
            {
                for (int i = 1; i <= DateTime.Now.Day; i++)
                {
                    var dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, i);
                    items.TryAdd(new StatisticDateResult(strategy, dateTime), new StatisticResultItem());
                }
            }
            else
            {
                for (int i = 1; i <= DateTime.Now.Month; i++)
                {
                    var dateTime = new DateTime(DateTime.Now.Year, i, 1);
                    items.TryAdd(new StatisticDateResult(strategy, dateTime), new StatisticResultItem());
                }
            }
            Details = items.ToDictionary(e => e.Key.ToString(), e => e.Value);
        }

        public class Builder
        {
            public StatisticStrategy Strategy { private get; init; }

            private SortedDictionary<StatisticDateResult, StatisticResultItem> _details;

            public StatisticResult Result
            {
                get => new(Strategy, _details);
            }

            public Builder(StatisticStrategy strategy)
            {
                Strategy = strategy;
                _details = new SortedDictionary<StatisticDateResult, StatisticResultItem>(StatisticDateResult.DefaultComparer);
            }

            public Builder AddItem(DateTime key, StatisticResultItem item)
            {
                _details.Add(new StatisticDateResult(Strategy, key), item);
                return this;
            }

            public Builder AddItem(int month, int year, StatisticResultItem item)
            {
                if (Strategy == StatisticStrategy.ByDay)
                {
                    throw new NotSupportedException(
                        $"This method does not supported for statistic {Strategy} strategy");
                }
                return AddItem(new DateTime(year, month, 1), item);
            }
        }
    }
}
