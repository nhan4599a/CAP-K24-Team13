using DatabaseAccessor.Models;
using Xunit;

namespace TestModel
{
    public class TestCartDetail
    {
        [Fact]
        public void Test()
        {
            var model = new CartDetail
            {
                Id = 123,
                CartId = 123,
                Quantity =123,
                ShopId = 123
            };
            Assert.Equal(123, model.Id);
            Assert.Equal(123, model.CartId);
            Assert.Equal(123, model.Quantity);
            Assert.Equal(123, model.ShopId);
        }
    }
}
