namespace Shared.Validations
{
    public enum FileValidationRuleName
    {
        ImageExtension,
        MinFileCount,
        MaxFileCount,
        SingleMaxFileSize,
        AllMaxFileSize
    }

    public static class FileValidationRuleNameExtensions
    {
        public static FileValidationRuleType GetFileValiationRuleType(this FileValidationRuleName ruleName)
        {
            return ruleName switch
            {
                FileValidationRuleName.ImageExtension or
                FileValidationRuleName.SingleMaxFileSize => FileValidationRuleType.Single,
                _ => FileValidationRuleType.Multiple
            };
        }
    }
}
