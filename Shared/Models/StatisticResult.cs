using System.Collections.Generic;

namespace Shared.Models
{
    public class StatisticResult<T>
    {
        public StatisticStrategy StatisticBy { get; set; }

        public SortedDictionary<StatisticDateResult, StatisticResultItem> Details { get; set; }

        public double HighestIncome { get; set; }

        public double LowestIncome { get; set; }

        public StatisticDateResult HighestDate { get; set; }

        public StatisticDateResult LowestDate { get; set; }

        public StatisticResult(StatisticStrategy statisticStrategy)
        {
            StatisticBy = statisticStrategy;
        }
    }
}
