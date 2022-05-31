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
    }
}