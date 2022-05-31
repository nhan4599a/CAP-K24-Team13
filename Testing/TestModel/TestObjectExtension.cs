using Shared.Extensions;
using Xunit;

namespace TestModel
{
    public class TestObjectExtension
    {
        [Fact]
        public void IsNumberTypeFailed()
        {
            var typeInt = typeof(int);

            var result = typeInt.IsNumberType();

            Assert.True(result);
        }

        [Fact]
        public void IsNumberTypeSuccess()
        {
            var typeInt = typeof(string);

            var result = typeInt.IsNumberType();

            Assert.False(result);
        }
    }
}
