using DatabaseAccessor.Models;
using Xunit;

namespace TestModel
{
    public class TestShopStatus
    {
        [Fact]
        public void Test()
        {
            var model = new ShopStatus
            {
                ShopId = 123,
                IsDisabled = true
            };
            Assert.Equal(123, model.ShopId);
            Assert.Equal(true, model.IsDisabled);

        }
    }
}
