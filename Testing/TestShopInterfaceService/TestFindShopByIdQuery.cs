using ShopInterfaceService.Commands;
using Xunit;

namespace TestShopInterfaceService
{
    public class TestFindShopInterfaceByShopIdQuery
    {
        [Fact]
        public void Test()
        {
            var model = new FindShopInterfaceByShopIdQuery
            {
                ShopId = 1
            };
            Assert.Equal(1, model.ShopId);
        }
    }
}
