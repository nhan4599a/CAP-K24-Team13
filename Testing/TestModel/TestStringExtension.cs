using Shared.Extensions;
using Xunit;

namespace TestModel
{
    public class TestStringExtension
    {
        [Fact]
        public void TestToBase64()
        {
            var plainText = "UnitTest";

            var expectedResult = "VW5pdFRlc3Q=";

            var result = plainText.ToBase64();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void TestFromBase64()
        {
            var encodedString = "VW5pdFRlc3Q=";

            var expectedResult = "UnitTest";

            var result = StringExtension.FromBase64(encodedString);

            Assert.Equal(expectedResult, result);
        }
    }
}
