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
                Amount = 20000,
                PartnerCode = "abc",
                OrderId = "abc",
                RequestId = "abc",
                OrderInfo = "abc",
                OrderType = "abc",
                TransId = "abc",
                ResultCode = 123,
                Message = "abc",
                PayType = "abc",
                ResponseTime = 1,
                ExtraData = "abc",
                Signature = "abc"



            };
            Assert.Equal("abc", model.AccessKey);
            Assert.Equal(20000, model.Amount);
            Assert.Equal("abc", model.PartnerCode);
            Assert.Equal("abc", model.OrderId);
            Assert.Equal("abc", model.RequestId);
            Assert.Equal("abc", model.OrderInfo);
            Assert.Equal("abc", model.OrderType);
            Assert.Equal(123, model.ResultCode);
            Assert.Equal("abc", model.Message);
            Assert.Equal("abc", model.PayType);
            Assert.Equal(1, model.ResponseTime);
            Assert.Equal("abc", model.ExtraData);
            Assert.Equal("abc", model.Signature);

        }
    }
}