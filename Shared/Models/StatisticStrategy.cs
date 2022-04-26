using System;

namespace Shared.Models
{
    public enum StatisticStrategy
    {
        ByDay, ByMonth, ByQuarter, ByYear
    }

    public static class StatisticStrategyExtensions
    {
        public static string GetStrategy(this StatisticStrategy strategy)
        {
            return "Statistic by " + strategy switch
            {
                StatisticStrategy.ByDay => "day",
                StatisticStrategy.ByMonth => "month",
                StatisticStrategy.ByQuarter => "quarter",
                StatisticStrategy.ByYear => "year",
                _ => throw new NotImplementedException()
            };
        }
    }
}
