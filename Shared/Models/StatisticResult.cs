using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

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

        [JsonPropertyName("user")]
        public int UsersCount { get; set; }

        protected StatisticResult(StatisticStrategy strategy, StatisticDateRange.Result range,
            SortedDictionary<StatisticDateResult, StatisticResultItem> items)
        {
            StatisticBy = strategy;
            Range = range;
            HighestIncome = items.Max(item => item.Value.Income);
            LowestIncome = items.Min(item => item.Value.Income);
            HighestDate = items.MaxBy(item => item.Value.Income).Key;
            LowestDate = items.MinBy(item => item.Value.Income).Key;
            if (StatisticBy == StatisticStrategy.ByDay)
            {
                while (Range.Start < Range.End)
                {
                    items.TryAdd(new StatisticDateResult(strategy, Range.Start), null);
                    Range.Start = Range.Start.AddDays(1);
                }
            }
            else if (StatisticBy == StatisticStrategy.ByMonth)
            {
                while ((Range.Start = Range.Start.AddMonths(1)) < Range.End)
                {
                    items.TryAdd(new StatisticDateResult(strategy, Range.Start), null);
                }
            }
            else if (StatisticBy == StatisticStrategy.ByQuarter)
            {
                while ((Range.Start = Range.Start.AddMonths(3)) < Range.End)
                {
                    items.TryAdd(new StatisticDateResult(strategy, Range.Start), null);
                }
            }
            else
            {
                while ((Range.Start = Range.Start.AddYears(1)) < Range.End)
                {
                    items.TryAdd(new StatisticDateResult(strategy, Range.Start), null);
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
