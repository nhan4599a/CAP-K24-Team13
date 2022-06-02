using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestRatingDTO
    {
        [Fact]
        public void Test()
        {
            var model = new RatingDTO
            {
                Id = "abc",
                ProductName = "abc",
                UserName = "abc",
                Message = "abc",
                Star = 123
            };
            Assert.Equal("abc", model.Id);
            Assert.Equal("abc", model.ProductName);
            Assert.Equal("abc", model.UserName);
            Assert.Equal("abc", model.Message);
            Assert.Equal(123, model.Star);

        }

    }
}
