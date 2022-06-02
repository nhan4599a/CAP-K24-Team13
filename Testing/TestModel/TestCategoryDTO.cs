using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestCategoryDTO
    {
        [Fact]
        public void Test()
        {
            var model = new CategoryDTO
            {
                CategoryId = 123,
                CategoryName = "abc",
                ProductCount = 123
            };
            Assert.Equal(123, model.CategoryId);
            Assert.Equal("abc", model.CategoryName);
            Assert.Equal(123, model.ProductCount);

        }
    }
}
