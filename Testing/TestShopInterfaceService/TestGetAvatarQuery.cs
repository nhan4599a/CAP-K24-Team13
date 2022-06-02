using ShopInterfaceService.Commands;
using Xunit;

namespace TestShopInterfaceService
{
    public class TestGetAvatarQuery
    {
        [Fact]
        public void Test()
        {
            var model = new GetShopAvatarQuery
            {
                ShopIds = new int[] { 1, 2, 3 }
            };
            Assert.NotEmpty(model.ShopIds);
        }
    }
}
