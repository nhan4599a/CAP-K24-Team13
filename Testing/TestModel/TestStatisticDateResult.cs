using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestStatisticDateResult
    {
        [Fact]
        public void TestConstructorStatisticDateResult()
        {
            var result = new StatisticDateResult(StatisticStrategy.ByDay, DateTime.Now);
            Assert.Equal(result.Result.Date, DateTime.Now.Date);
            Assert.Equal(StatisticStrategy.ByDay, result.Strategy);

            Assert.Equal(DateTime.Now.ToString("dd/MM/yyyy"), result.ToString());

            result = new StatisticDateResult(StatisticStrategy.ByMonth, DateTime.Now);
            Assert.Equal(result.Result.Date, DateTime.Now.Date);
            Assert.Equal(StatisticStrategy.ByMonth, result.Strategy);

            Assert.Equal("06/2022", result.ToString());

            result = new StatisticDateResult(StatisticStrategy.ByQuarter, new DateTime(2022, 4, 20));
            Assert.Equal(result.Result.Date, new DateTime(2022, 4, 20).Date);
            Assert.Equal(StatisticStrategy.ByQuarter, result.Strategy);

            Assert.Equal("4/2022", result.ToString());

            result = new StatisticDateResult(StatisticStrategy.ByYear, DateTime.Now);
            Assert.Equal(result.Result.Date, DateTime.Now.Date);
            Assert.Equal(StatisticStrategy.ByYear, result.Strategy);

            Assert.Equal("2022", result.ToString());
        }
    }
}
