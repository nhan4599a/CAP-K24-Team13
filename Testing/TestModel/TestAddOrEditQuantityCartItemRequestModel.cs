using Shared.RequestModels;
using Xunit;

namespace TestModel
{
    public class TestAddOrEditQuantityCartItemRequestModel
    {
        [Fact]
        public void Test()
        {
            var model = new AddOrEditQuantityCartItemRequestModel
            {
                ProductId = "abc",
                Quantity = 123,
                UserId = "abc"
            };
            Assert.Equal("abc", model.ProductId);
            Assert.Equal(123, model.Quantity);
            Assert.Equal("abc", model.UserId);
        }
    }
}
