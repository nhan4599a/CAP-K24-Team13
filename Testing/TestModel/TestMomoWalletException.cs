using GUI.Payments.Momo.Exceptions;
using Xunit;

namespace TestModel
{
    public class TestMomoWalletException
    {
        [Fact]
        public void Test()
        {
            var model = new MomoWalletException
            {
                Field = "abc"
            };
            Assert.Equal("abc", model.Field);
        }
    }
}
