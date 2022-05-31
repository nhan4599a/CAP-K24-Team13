using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestStatisticResultBuilder
    {
        [Fact]
        public void TestStatisticResultBuilderSucceed()
        {
            var builder = new StatisticResult.Builder(StatisticStrategy.ByDay);
            builder.AddItem(new DateTime(2021, 1, 1), new StatisticResultItem
            {
                Income = 200000,
                Data = new StatisticResultItemData
                {
                    CanceledInvoiceCount = 1,
                    NewInvoiceCount = 2,
                    SucceedInvoiceCount = 1
                }
            });
            builder.AddItem(new DateTime(2021, 1, 2), new StatisticResultItem
            {
                Income = 200000,
                Data = new StatisticResultItemData
                {
                    CanceledInvoiceCount = 1,
                    NewInvoiceCount = 2,
                    SucceedInvoiceCount = 1
                }
            });
            builder.AddItem(new DateTime(2021, 1, 3), new StatisticResultItem
            {
                Income = 200000,
                Data = new StatisticResultItemData
                {
                    CanceledInvoiceCount = 1,
                    NewInvoiceCount = 2,
                    SucceedInvoiceCount = 1
                }
            });
            var statisticDateRange = new StatisticDateRange(StatisticStrategy.ByDay, "01/01/2021", "03/01/2021");
            var result = builder.Build(statisticDateRange);
            Assert.NotNull(result);
            Assert.Equal(200000, result.HighestIncome);
            Assert.Equal(200000, result.LowestIncome);
            var highestDate = result.HighestDate;
            var lowestDate = result.LowestDate;
            Assert.NotNull(highestDate);
            Assert.NotNull(lowestDate);
            Assert.Equal(1, highestDate.Result.Day);
            Assert.Equal(1, highestDate.Result.Month);
            Assert.Equal(2021, highestDate.Result.Year);
            Assert.Equal(1, lowestDate.Result.Day);
            Assert.Equal(1, lowestDate.Result.Month);
            Assert.Equal(2021, lowestDate.Result.Year);
        }
    }
}
