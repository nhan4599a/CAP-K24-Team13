using Shared.Validations;
using Xunit;

namespace TestModel
{
    public class TestFileValidationRule
    {
        [Fact]
        public void Test()
        {
            Assert.NotEmpty(FileValidationRule.IMAGE_EXTENSIONS);
            Assert.Contains("jpg", FileValidationRule.IMAGE_EXTENSIONS);
            Assert.Contains("png", FileValidationRule.IMAGE_EXTENSIONS);
            Assert.Contains("jpeg", FileValidationRule.IMAGE_EXTENSIONS);
        }
    }
}
