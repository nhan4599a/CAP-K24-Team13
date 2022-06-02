using DatabaseAccessor.Models;
using Xunit;

namespace TestModel
{
    public class TestCartDetail
    {
        [Fact]
        public void Test()
        {
            var guid = Guid.NewGuid();
            var model = new CartDetail
            {
                Id = 123,
                CartId = 123,
                Quantity = 123,
                ShopId = 123,
                ProductId = guid,
                Cart = new Cart(),
                Product = new ShopProduct()
            };
            Assert.Equal(123, model.Id);
            Assert.Equal(123, model.CartId);
            Assert.Equal(123, model.Quantity);
            Assert.Equal(123, model.ShopId);
            Assert.Equal(guid, model.ProductId);
            Assert.NotNull(model.Cart);
            Assert.NotNull(model.Product);
        }
    }
}
