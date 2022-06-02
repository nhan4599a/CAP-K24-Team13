using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestShopDTO
    {
        [Fact]
        public void Test()
        {
            var model = new ShopDTO
            {
                Id = 123,
                ShopName = "abc",
                UserId = "abc",
                IsAvailable = true,
                Name = "abc",
                Phone = "abc",
                Email = "abc",
                Description = "abc",
                Avatar = "abc",
                Floor = "abc",
                Position = "abc",
                Images = new string[] { "1.png", "2.png" }
            };
            Assert.Equal(123, model.Id);
            Assert.Equal("abc", model.ShopName);
            Assert.Equal("abc", model.UserId);
            Assert.Equal("abc", model.Name);
            Assert.Equal("abc", model.Phone);
            Assert.Equal("abc", model.Email);
            Assert.Equal("abc", model.Description);
            Assert.Equal("abc", model.Avatar);
            Assert.Equal("abc", model.Floor);
            Assert.Equal("abc", model.Position);
            Assert.True(model.IsAvailable);
            Assert.NotEmpty(model.Images);
        }
    }
}
