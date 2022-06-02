using Shared.Extensions;
using Xunit;

namespace TestModel
{
    public class TestDateTimeExtensions
    {
        [Fact]
        public void TestTryParseExactSuccess()
        {
            var dateTimeString = "21/12/2021";
            var parseResult = DateTimeExtension.TryParseExact(dateTimeString, "dd/MM/yyyy", out DateTime dateTime);

            Assert.True(parseResult);

            Assert.Equal(21, dateTime.Day);
            Assert.Equal(12, dateTime.Month);
            Assert.Equal(2021, dateTime.Year);
        }

        [Fact]
        public void TestTryParseExactFailed()
        {
            var dateTimeString = "21/12/2021";
            var parseResult = DateTimeExtension.TryParseExact(dateTimeString, "MM/dd/yyyy", out DateTime dateTime);

            Assert.False(parseResult);
            Assert.Equal(DateTime.MinValue, dateTime);
        }

        [Fact]
        public void TestStartOfMonth()
        {
            var dateTime = DateTimeExtension.StartOfMonth(1, 2021);
            Assert.Equal(1, dateTime.Day);
            Assert.Equal(1, dateTime.Month);
            Assert.Equal(2021, dateTime.Year);

            var exception = Assert.Throws<ArgumentException>(() => DateTimeExtension.StartOfMonth(0, 2021));
            Assert.NotNull(exception);
            Assert.Equal("0 is not a valid month", exception.Message);

            exception = Assert.Throws<ArgumentException>(() => DateTimeExtension.StartOfMonth(13, 2021));
            Assert.NotNull(exception);
            Assert.Equal("13 is not a valid month", exception.Message);
        }

        [Fact]
        public void TestEndOfMonth()
        {
            var dateTime = DateTimeExtension.EndOfMonth(1, 2021);
            Assert.Equal(31, dateTime.Day);
            Assert.Equal(1, dateTime.Month);
            Assert.Equal(2021, dateTime.Year);

            var exception = Assert.Throws<ArgumentException>(() => DateTimeExtension.EndOfMonth(0, 2021));
            Assert.NotNull(exception);
            Assert.Equal("0 is not a valid month", exception.Message);

            exception = Assert.Throws<ArgumentException>(() => DateTimeExtension.EndOfMonth(13, 2021));
            Assert.NotNull(exception);
            Assert.Equal("13 is not a valid month", exception.Message);
        }

        [Fact]
        public void TestGetQuarter()
        {
            var quarter = DateTimeExtension.GetQuarter(12);
            Assert.Equal(4, quarter);

            var exception = Assert.Throws<ArgumentException>(() => DateTimeExtension.GetQuarter(13));
            Assert.NotNull(exception);
            Assert.Equal("13 is not a valid month", exception.Message);

            exception = Assert.Throws<ArgumentException>(() => DateTimeExtension.GetQuarter(0));
            Assert.NotNull(exception);
            Assert.Equal("0 is not a valid month", exception.Message);
        }

        [Fact]
        public void TestDayOfMonth()
        {
            var dayCount = DateTimeExtension.DayOfMonth(12, 2021);
            Assert.Equal(31, dayCount);

            var exception = Assert.Throws<ArgumentException>(() => DateTimeExtension.DayOfMonth(13, 2021));
            Assert.NotNull(exception);
            Assert.Equal("13 is not a valid month", exception.Message);

            exception = Assert.Throws<ArgumentException>(() => DateTimeExtension.DayOfMonth(0, 2021));
            Assert.NotNull(exception);
            Assert.Equal("0 is not a valid month", exception.Message);
        }
    }
}