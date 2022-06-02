using ShopInterfaceService.Commands;
using Xunit;

namespace TestShopInterfaceService
{
    public class TestGetShopInterfaceQuery
    {
        [Fact]
        public void Test()
        {
            var model = new GetShopInterfacesQuery
            {
                ShopIds = new List<int> { 1, 2, 3 }
            };
            Assert.NotEmpty(model.ShopIds);
        }
    }
}
