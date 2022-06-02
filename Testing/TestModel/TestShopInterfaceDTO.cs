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
                Avatar = "abc"
            };
            Assert.Equal(123, model.ShopId);
            Assert.Equal("abc", model.Avatar);
        }
    }
}
