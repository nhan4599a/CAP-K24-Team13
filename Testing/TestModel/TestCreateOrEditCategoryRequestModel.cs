using Shared.RequestModels;
using Xunit;

namespace TestModel
{
    public class TestCreateOrEditCategoryRequestModel
    {
        [Fact]
        public void Test()
        {
            var model = new CreateOrEditCategoryRequestModel
            {
                CategoryName = "abc",
                ImagePath = "abc"
            };
            Assert.Equal("abc", model.CategoryName);
            Assert.Equal("abc", model.ImagePath);

        }
    }
}
