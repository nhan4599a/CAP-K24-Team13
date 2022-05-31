using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestMomoWalletIpnRequest
    {
        [Fact]
        public void Test()
        {
            var model = new MomoWalletIpnRequest
            {
                AccessKey = "abc",
                Amount = 20000
            };
            Assert.Equal("abc", model.AccessKey);
            Assert.Equal(20000, model.Amount);
        }
    }
}