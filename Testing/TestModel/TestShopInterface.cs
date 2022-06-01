using DatabaseAccessor.Models;
using Xunit;
namespace TestModel
{
    public class TestShopInterface
    {
        [Fact]
        public void Test()
        {
            var model = new ShopInterface
            {
                ShopId = 123,
                Avatar = "abc",
                Images = "abc",
                IsVisible = true 
            };
            Assert.Equal(123, model.ShopId);
            Assert.Equal("abc", model.Avatar);
            Assert.Equal("abc", model.Images);
            Assert.Equal(true, model.IsVisible);

        }
    }
}
