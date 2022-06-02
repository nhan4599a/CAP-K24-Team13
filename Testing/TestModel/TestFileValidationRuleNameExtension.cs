using Shared.Validations;
using Xunit;

namespace TestModel
{
    public class TestFileValidationRuleNameExtension
    {
        [Fact]
        public void Test()
        {
            var ruleType = FileValidationRuleName.ImageExtension.GetFileValiationRuleType();
            Assert.Equal(FileValidationRuleType.Single, ruleType);
            ruleType = FileValidationRuleName.MaxFileCount.GetFileValiationRuleType();
            Assert.Equal(FileValidationRuleType.Multiple, ruleType);
        }
    }
}
