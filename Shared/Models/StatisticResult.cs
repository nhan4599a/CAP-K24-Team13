using Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Models
{
    public class StatisticResult
    {
        public StatisticStrategy StatisticBy { get; set; }

        public StatisticDateRange.Result Range { get; set; }

        public IDictionary<string, StatisticResultItem> Details { get; set; }

        public double HighestIncome { get; set; }

        public double LowestIncome { get; set; }

        public StatisticDateResult HighestDate { get; set; }

        public StatisticDateResult LowestDate { get; set; }

        protected StatisticResult(StatisticStrategy strategy, StatisticDateRange.Result range,
            SortedDictionary<StatisticDateResult, StatisticResultItem> items)
        {
            StatisticBy = strategy;
            Range = range;
            HighestIncome = items.Count > 0 ? items.Max(item => item.Value.Income) : 0;
            LowestIncome = items.Count > 0 ? items.Min(item => item.Value.Income) : 0;
            HighestDate = items.Count > 0 ? items.MaxBy(item => item.Value.Income).Key : null;
            LowestDate = items.Count > 0 ? items.MinBy(item => item.Value.Income).Key : null;
            var startDate = Range.Start;
            if (StatisticBy == StatisticStrategy.ByDay)
            {
                while (startDate < Range.End)
                {
                    items.TryAdd(new StatisticDateResult(strategy, startDate), null);
                    startDate = startDate.AddDays(1);
                }
            }
            else if (StatisticBy == StatisticStrategy.ByMonth)
            {
                while (startDate < Range.End)
                {
                    items.TryAdd(new StatisticDateResult(strategy, startDate), null);
                    startDate = startDate.AddMonths(1);
                }
            }
            else if (StatisticBy == StatisticStrategy.ByQuarter)
            {
                while (startDate < Range.End)
                {
                    int quarter = DateTimeExtension.GetQuarter(startDate.Month);
                    items.TryAdd(new StatisticDateResult(strategy, 
                        DateTimeExtension.StartOfMonth(quarter, startDate.Year)), null);
                    startDate = startDate.AddMonths(3);
                }
            }
            else
            {
                while (startDate < Range.End)
                {
                    items.TryAdd(new StatisticDateResult(strategy, startDate), null);
                    startDate = startDate.AddYears(1);
                }
            }
            Details = items.ToDictionary(e => e.Key.ToString(), e => e.Value);
        }

        public class Builder
        {
            public StatisticStrategy Strategy { private get; init; }

            private SortedDictionary<StatisticDateResult, StatisticResultItem> _details;

            public Builder(StatisticStrategy strategy)
            {
                Strategy = strategy;
                _details = 
                    new SortedDictionary<StatisticDateResult, StatisticResultItem>(StatisticDateResult.DefaultComparer);
            }

            public Builder AddItem(DateTime key, StatisticResultItem item)
            {
                _details.Add(new StatisticDateResult(Strategy, key), item);
                return this;
            }

            public Builder AddItem(int month, int year, StatisticResultItem item)
            {
                return AddItem(new DateTime(year, month, 1), item);
            }

            public Builder AddItem(int year, StatisticResultItem item)
            {
                return AddItem(new DateTime(year, 1, 1), item);
            }

            public StatisticResult Build(StatisticDateRange range)
            {
                return new(Strategy, range.Range, _details);
            }
        }
    }
}
