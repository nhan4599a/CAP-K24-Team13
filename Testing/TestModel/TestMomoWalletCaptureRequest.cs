
using GUI.Payments.Momo.Models;
using Xunit;

namespace TestModel
{
    public class TestMomoWalletCaptureRequest
    {
        [Fact]
        public void Test()
        {
            var model = new MomoWalletCaptureRequest
            {
                PartnerCode = "abc",
                OrderId = "abc",
                RequestId = "abc",
                Amount = 123,
                AccessKey = "abc",
                IpnUrl = "abc",
                RedirectUrl = "abc",
                OrderInfo = "abc",
                ResponseLanguage = "abc",
                ExtraData = "abc",
                Signature = "abc",
            };
            Assert.Equal("abc", model.PartnerCode);
            Assert.Equal("abc", model.OrderId);
            Assert.Equal("abc", model.RequestId);
            Assert.Equal(123, model.Amount);
            Assert.Equal("abc", model.AccessKey);
            Assert.Equal("abc", model.IpnUrl);
            Assert.Equal("abc", model.RedirectUrl);
            Assert.Equal("abc", model.OrderInfo);
            Assert.Equal("abc", model.ExtraData);
            Assert.Equal("abc", model.Signature);
        }
    }
}
