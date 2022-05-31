using Shared.Validations;
using Xunit;

namespace TestModel
{
    public class TestFileValidationRuleSet
    {
        [Fact]
        public void TestAddRule()
        {
            var ruleSet = new FileValidationRuleSet();
            var fileValidationRule = new FileValidationRule
            {
                RuleName = FileValidationRuleName.MinFileCount,
                Value = 2
            };
            ruleSet.Add(fileValidationRule);
            Assert.Single(ruleSet);
            var newValidationRule = new FileValidationRule
            {
                RuleName = FileValidationRuleName.MinFileCount,
                Value = 3
            };
            Assert.Throws<InvalidOperationException>(() => ruleSet.Add(newValidationRule));
        }

        [Fact]
        public void TestDeleteRule()
        {
            var rules = new HashSet<FileValidationRule>();
            var fileValidationRule = new FileValidationRule
            {
                RuleName = FileValidationRuleName.MinFileCount,
                Value = 2
            };
            rules.Add(fileValidationRule);
            rules.Add(new FileValidationRule
            {
                RuleName = FileValidationRuleName.SingleMaxFileSize,
                Value = 1024
            });
            var ruleSet = new FileValidationRuleSet(rules);
            ruleSet.Remove(FileValidationRuleName.SingleMaxFileSize);
            Assert.Single(ruleSet);
            Assert
                .Throws<InvalidOperationException>(() => ruleSet.Remove(FileValidationRuleName.SingleMaxFileSize));
        }

        [Fact]
        public void TestFileValidationRuleSetIndexer()
        {
            var rules = new HashSet<FileValidationRule>();
            var fileValidationRule = new FileValidationRule
            {
                RuleName = FileValidationRuleName.MinFileCount,
                Value = 2
            };
            rules.Add(fileValidationRule);
            rules.Add(new FileValidationRule
            {
                RuleName = FileValidationRuleName.SingleMaxFileSize,
                Value = 1024
            });
            var ruleSet = new FileValidationRuleSet(rules);

            var result = ruleSet[FileValidationRuleName.MinFileCount];
            Assert.NotNull(result);
            Assert.Equal(FileValidationRuleName.MinFileCount, result.RuleName);
            Assert.Equal(2, result.Value);

            var nullResult = ruleSet[FileValidationRuleName.ImageExtension];
            Assert.Null(nullResult);
        }
    }
}