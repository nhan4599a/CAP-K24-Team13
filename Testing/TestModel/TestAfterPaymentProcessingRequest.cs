using Shared.Models;
using Xunit;


namespace TestModel
{
    public class TestAfterPaymentProcessingRequest

    {
        [Fact]
        public void Test()
        {
            var model = new AfterPaymentProcessingRequest
            {
                AccessToken = "abc",
                WalletIpnRequest ="abc"
            };
            Assert.Equal("abc", model.AccessToken);
            Assert.Equal("abc", model.WalletIpnRequest);
        }
    }
}
