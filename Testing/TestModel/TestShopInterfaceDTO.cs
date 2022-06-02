using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestShopInterfaceDTO
    {
        [Fact]
        public void Test()
        {
            var model = new ShopInterfaceDTO
            {
                ShopId = 123,
                Avatar = "abc",
                Images = new string[] { "1.png", "2.png" }
            };
            Assert.Equal(123, model.ShopId);
            Assert.Equal("abc", model.Avatar);
            Assert.NotEmpty(model.Images);
        }
    }
}
