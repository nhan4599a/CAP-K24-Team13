using Shared.Exceptions;
using Shared.Validations;
using Xunit;

namespace TestModel
{
    public class TestImageValidationException
    {
        [Fact]
        public void TestSucceed()
        {
            var fileValidationRuleSet = new FileValidationRuleSet
            {
                new FileValidationRule
                {
                    RuleName = FileValidationRuleName.MinFileCount,
                    Value = 1
                },
                new FileValidationRule
                {
                    RuleName = FileValidationRuleName.SingleMaxFileSize,
                    Value = 1024
                }
            };
            var passedRules = new List<FileValidationRuleName> { FileValidationRuleName.SingleMaxFileSize };
            var fileValidationResult = FileValidationResult.CreateFailedResult(fileValidationRuleSet, passedRules);

            var fileValidationException = new FileValidationException(fileValidationResult);
            Assert.NotNull(fileValidationException.ValidationResult);
            Assert.NotEmpty(fileValidationException.Message);
            Assert.Equal("Following file validation rules are violated [MinFileCount]", fileValidationException.Message);
        }

        [Fact]
        public void TestFailed()
        {
            var fileValidationRuleSet = new FileValidationRuleSet
            {
                new FileValidationRule
                {
                    RuleName = FileValidationRuleName.MinFileCount,
                    Value = 1
                },
                new FileValidationRule
                {
                    RuleName = FileValidationRuleName.SingleMaxFileSize,
                    Value = 1024
                }
            };
            var fileValidationResult = FileValidationResult.CreateSucceedResult(fileValidationRuleSet);

            var exception = Assert.Throws<ArgumentException>(() => new FileValidationException(fileValidationResult));
            Assert.NotNull(exception);
            Assert.Equal("ValidationResult is succeed", exception.Message);
        }
    }
}