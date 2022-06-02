using Shared.RequestModels;
using Xunit;

namespace TestModel
{
    public class TestCreateProductRequestModel
    {
        [Fact]
        public void Test()
        {
            var model = new CreateProductRequestModel
            {
                Quantity = 123
            };
            Assert.Equal(123, model.Quantity);
        }
    }
}
