using Shared.RequestModels;
using Xunit;

namespace TestModel
{
    public class TestCheckOutRequestModel
    {
        [Fact]
        public void Test()
        {
            var model = new CheckOutRequestModel
            {
                UserId = "abc",
                ProductIds = "abc",
                ShippingName = "abc",
                ShippingPhone = "abc",
                ShippingAddress = "abc",
                OrderNotes = "abc"
            };
            Assert.Equal("abc", model.UserId);
            Assert.Equal("abc", model.ProductIds);
            Assert.Equal("abc", model.ShippingName);
            Assert.Equal("abc", model.ShippingPhone);
            Assert.Equal("abc", model.ShippingAddress);
            Assert.Equal("abc", model.OrderNotes);

        }
    }
}
