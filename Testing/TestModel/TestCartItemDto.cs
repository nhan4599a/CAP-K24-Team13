using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestCartItemDto
    {
        [Fact]
        public void Test()
        {
            var model = new CartItemDTO
            {
                ProductId = "abc",
                ProductName = "abc",
                Price = 123,
                Discount = 123,
                Quantity = 123,
                Image = "abc",
                IsAvailable = true
            };
            Assert.True(model.IsAvailable);
            Assert.Equal("abc", model.ProductId);
            Assert.Equal("abc", model.ProductName);
            Assert.Equal(123, model.Price);
            Assert.Equal(123, model.Discount);
            Assert.Equal("abc", model.Image);
            Assert.Equal(123, model.Quantity);
        }
    }
}
