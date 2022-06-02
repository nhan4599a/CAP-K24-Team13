using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestProductDTO
    {
        [Fact]
        public void Test()
        {
            var model = new ProductDTO
            {
                Description = "abc",
                CategoryId = 123,
                CategoryName = "abc",
                IsNewProduct = true,
                AverageRating = 123
            };
            Assert.True(model.IsNewProduct);
            Assert.Equal("abc", model.Description);
            Assert.Equal(123, model.CategoryId);
            Assert.Equal("abc", model.CategoryName);
            Assert.Equal(123, model.AverageRating);

        }
    }
}
