using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestStatisticStrategyExtension
    {
        [Fact]
        public void Test()
        {
            var byDay = StatisticStrategy.ByDay.GetStrategy();
            var byMonth = StatisticStrategy.ByMonth.GetStrategy();
            var byQuarter = StatisticStrategy.ByQuarter.GetStrategy();
            var byYear = StatisticStrategy.ByYear.GetStrategy();

            Assert.Equal("Statistic by " + "day", byDay);
            Assert.Equal("Statistic by " + "month", byMonth);
            Assert.Equal("Statistic by " + "quarter", byQuarter);
            Assert.Equal("Statistic by " + "year", byYear);
        }
    }
}
