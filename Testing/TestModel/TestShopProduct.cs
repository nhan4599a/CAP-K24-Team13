using DatabaseAccessor.Models;
using Xunit;

namespace TestModel
{
    public class TestShopProduct
    {
        [Fact]
        public void Test()
        {
            var model = new ShopProduct
            {
                ProductName = "abc",
                Description = "abc",
                Images = "abc",
                Quantity = 123,
                Price = 123,
                Discount = 123,
                IsDisabled = false,
                CategoryId = 123,
                ShopId = 123,
                IsVisible = true,
                Category = "abc"
            };
            Assert.Equal("abc", model.ProductName);
            Assert.Equal("abc", model.Description);
            Assert.Equal("abc", model.Images);
            Assert.Equal(123, model.Quantity);
            Assert.Equal(123, model.Price);
            Assert.Equal(123, model.Discount);
            Assert.False(model.IsDisabled);
            Assert.Equal(123, model.CategoryId);
            Assert.Equal(123, model.ShopId);
            Assert.True(model.IsVisible);
            Assert.Equal("abc", model.Category);

        }
    }
}
