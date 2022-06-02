using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestMinimalProductDTO
    {
        [Fact]
        public void Test()
        {
            var model = new MinimalProductDTO
            {
                Id = "abc",
                ProductName = "abc",
                Price = 123,
                Discount = 123,
                Quantity = 123,
                ShopId = 123,
                IsAvailable= true
            };
            Assert.True(model.IsAvailable);
            Assert.Equal("abc", model.Id);
            Assert.Equal(123, model.Price);
            Assert.Equal("abc", model.ProductName);
            Assert.Equal(123, model.Discount);
            Assert.Equal(123, model.Quantity);
            Assert.Equal(123, model.ShopId);

        }
    }
}
