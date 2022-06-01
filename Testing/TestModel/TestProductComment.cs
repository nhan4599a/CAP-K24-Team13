using DatabaseAccessor.Models;
using Xunit;
namespace TestModel
{
    public class TestProductComment
    {
        [Fact]
        public void Test()
        {
            var model = new ProductComment
            {
                Id = 123,
                Message = "abc",
                Star = 5
            };
            Assert.Equal(123, model.Id);
            Assert.Equal("abc", model.Message);
            Assert.Equal(5, model.Star);
        }
    }
}
