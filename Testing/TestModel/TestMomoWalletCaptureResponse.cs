using GUI.Payments.Momo.Models;
using Xunit;

namespace TestModel
{
    public class TestMomoWalletCaptureResponse
    {
        [Fact]
        public void Test()
        {
            var model = new MomoWalletCaptureResponse
            {
                PartnerCode = "abc",
                OrderId = "abc",
                RequestId = "abc",
                Amount = 123,
                ResponseTime =1,
                Message = "abc",
                ResultCode = 123,
                PayUrl = "abc"
            };
            Assert.Equal("abc", model.PartnerCode);
            Assert.Equal("abc", model.RequestId);
            Assert.Equal("abc", model.OrderId);
            Assert.Equal(123, model.Amount);
            Assert.Equal(1, model.ResponseTime);
            Assert.Equal("abc", model.Message);
            Assert.Equal(123, model.ResultCode);
            Assert.Equal("abc", model.PayUrl);
        }
    }
}
