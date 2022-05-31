using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestStatisticDateRange
    {
        [Fact]
        public void TestConstructorDate()
        {
            var statisticDateRange =
                new StatisticDateRange(StatisticStrategy.ByDay, "15/01/2021", "20/01/2021");

            var range = statisticDateRange.Range;
            Assert.NotNull(range);
            Assert.Equal(StatisticStrategy.ByDay, statisticDateRange.Strategy);
            var startDate = range.Start;
            var endDate = range.End;
            Assert.Equal(15, startDate.Day);
            Assert.Equal(1, startDate.Month);
            Assert.Equal(2021, startDate.Year);
            Assert.Equal(20, endDate.Day);
            Assert.Equal(1, endDate.Month);
            Assert.Equal(2021, endDate.Year);
        }

        [Fact]
        public void TestConstructorMonth()
        {
            var statisticDateRange =
                new StatisticDateRange(StatisticStrategy.ByMonth, "01/2021", "01/2021");

            var range = statisticDateRange.Range;
            Assert.NotNull(range);
            Assert.Equal(StatisticStrategy.ByMonth, statisticDateRange.Strategy);
            var startDate = range.Start;
            var endDate = range.End;
            Assert.Equal(1, startDate.Day);
            Assert.Equal(1, startDate.Month);
            Assert.Equal(2021, startDate.Year);
            Assert.Equal(31, endDate.Day);
            Assert.Equal(1, endDate.Month);
            Assert.Equal(2021, endDate.Year);

            statisticDateRange = new StatisticDateRange(StatisticStrategy.ByMonth, "01/2021", "02/2021");
            range = statisticDateRange.Range;
            startDate = range.Start;
            endDate = range.End;
            Assert.Equal(1, startDate.Day);
            Assert.Equal(1, startDate.Month);
            Assert.Equal(2021, startDate.Year);
            Assert.Equal(28, endDate.Day);
            Assert.Equal(2, endDate.Month);
            Assert.Equal(2021, endDate.Year);
        }

        [Fact]
        public void TestConstructorQuarter()
        {
            var statisticDateRange = new StatisticDateRange(StatisticStrategy.ByQuarter, "1/2021", "4/2021");
            var range = statisticDateRange.Range;
            Assert.NotNull(range);
            Assert.Equal(StatisticStrategy.ByQuarter, statisticDateRange.Strategy);
            var startDate = range.Start;
            var endDate = range.End;
            Assert.Equal(1, startDate.Day);
            Assert.Equal(1, startDate.Month);
            Assert.Equal(2021, startDate.Year);
            Assert.Equal(31, endDate.Day);
            Assert.Equal(12, endDate.Month);
            Assert.Equal(2021, endDate.Year);
        }

        [Fact]
        public void TestConstructorYear()
        {
            var statisticDateRange = new StatisticDateRange(StatisticStrategy.ByYear, "2021", "2021");
            var range = statisticDateRange.Range;
            Assert.NotNull(range);
            Assert.Equal(StatisticStrategy.ByYear, statisticDateRange.Strategy);
            var startDate = range.Start;
            var endDate = range.End;
            Assert.Equal(1, startDate.Day);
            Assert.Equal(1, startDate.Month);
            Assert.Equal(2021, startDate.Year);
            Assert.Equal(31, endDate.Day);
            Assert.Equal(12, endDate.Month);
            Assert.Equal(2021, endDate.Year);
        }

        [Fact]
        public void TestConstructorDayFailedBecauseLessThan()
        {
            Assert
                .Throws<ArgumentException>(
                    () => new StatisticDateRange(StatisticStrategy.ByDay, "01/01/2021", "01/02/2018"));
        }

        [Fact]
        public void TestConstructorMonthFailedBecauseLessThan()
        {
            Assert
                .Throws<ArgumentException>(
                    () => new StatisticDateRange(StatisticStrategy.ByMonth, "01/2021", "12/2018"));
        }

        [Fact]
        public void TestConstructorQuarterFailedBecauseLessThan()
        {
            Assert
                .Throws<ArgumentException>(
                    () => new StatisticDateRange(StatisticStrategy.ByQuarter, "1/2021", "3/2018"));
        }

        [Fact]
        public void TestConstructorYearFailedBecauseLessThan()
        {
            Assert
                .Throws<ArgumentException>(() => new StatisticDateRange(StatisticStrategy.ByYear, "2021", "2018"));
        }

        [Fact]
        public void TestConstructorDayFailedBecauseInvalid()
        {
            Assert
                .Throws<ArgumentException>(
                    () => new StatisticDateRange(StatisticStrategy.ByDay, "01/14/2021", "12/12/2022"));
        }

        [Fact]
        public void TestConstructorMonthFailedBecauseInvalid()
        {
            Assert
                .Throws<ArgumentException>(
                    () => new StatisticDateRange(StatisticStrategy.ByMonth, "14/2021", "12/2022"));
        }

        [Fact]
        public void TestConstructorQuarterFailedBecauseInvalid()
        {
            Assert
                .Throws<ArgumentException>(
                    () => new StatisticDateRange(StatisticStrategy.ByQuarter, "5/2021", "4/2022"));
        }

        [Fact]
        public void TestConstructorYearFailedBecauseInvalid()
        {
            Assert
                .Throws<ArgumentException>(
                    () => new StatisticDateRange(StatisticStrategy.ByYear, "2021", "202"));
        }

        [Fact]
        public void TestTryCreateSuccess()
        {
            var result = 
                StatisticDateRange
                    .TryCreate(StatisticStrategy.ByDay, "01/12/2021", "15/01/2022", out StatisticDateRange range);

            Assert.True(result.IsSucceed);
            Assert.NotNull(range);
            Assert.Null(result.Exception);

            var startDate = range.Range.Start;
            var endDate = range.Range.End;

            Assert.Equal(1, startDate.Day);
            Assert.Equal(12, startDate.Month);
            Assert.Equal(2021, startDate.Year);
            Assert.Equal(15, endDate.Day);
            Assert.Equal(1, endDate.Month);
            Assert.Equal(2022, endDate.Year);
        }

        [Fact]
        public void TestTryCreateFailed()
        {
            var result =
                StatisticDateRange
                    .TryCreate(StatisticStrategy.ByDay, "01/13/2022", "13/01/2023", out StatisticDateRange range);

            Assert.False(result.IsSucceed);
            Assert.Null(range);
            Assert.NotNull(result.Exception);
        }

        [Fact]
        public void TestStatisticDateRangeResult()
        {
            var succeedResult = StatisticDateRange.ParseResult.SucceedResult;

            Assert.True(succeedResult.IsSucceed);
            Assert.Null(succeedResult.Exception);

            var failedResult = StatisticDateRange.ParseResult.CreateErrorResult(new Exception());

            Assert.False(failedResult.IsSucceed);
            Assert.NotNull(failedResult.Exception);
        }
    }
}
