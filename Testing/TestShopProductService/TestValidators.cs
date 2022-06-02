using ShopProductService.Validations;
using Xunit;

namespace TestShopProductService
{
    public class TestValidators
    {
        [Fact]
        public void TestCreateProductRequestModelValidator()
        {
            _ = new CreateProductRequestModelValidator();
        }

        [Fact]
        public void TestEditProductRequestModelValidator()
        {
            _ = new EditProductRequestModelValidator();
        }

        [Fact]
        public void TestSearchProductRequestModelValidator()
        {
            _ = new SearchProductRequestModelValidator();
        }
    }
}