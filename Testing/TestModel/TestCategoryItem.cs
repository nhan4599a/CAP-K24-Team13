using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestCategoryItem
    {
        [Fact]
        public void Test()
        {
            var model = new CategoryItem
            {
                Id = 123,
                Name = "abc"
            };
            Assert.Equal(123, model.Id);
            Assert.Equal("abc", model.Name);
        }
        
    }
}
