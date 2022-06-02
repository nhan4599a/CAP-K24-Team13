using Shared.RequestModels;
using Xunit;

namespace TestModel
{
    public class TestEditProductRequestModel
    {
        [Fact]
        public void Test()
        {
            var model = new EditProductRequestModel
            {
                ProductName = "abc",
                CategoryId = 123,
                CategoryName = "abc",
                Description = "abc",
                Price = 123,
                Discount = 123,
                ShopId = 123
            };
            Assert.Equal("abc", model.ProductName);
            Assert.Equal(123, model.CategoryId);
            Assert.Equal("abc", model.CategoryName);
            Assert.Equal("abc", model.Description);
            Assert.Equal(123, model.Price);
            Assert.Equal(123, model.Discount);
            Assert.Equal(123, model.ShopId);

        }
    }
}
