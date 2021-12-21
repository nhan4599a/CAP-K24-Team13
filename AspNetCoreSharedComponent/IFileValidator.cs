using Shared.Validations;

namespace AspNetCoreSharedComponent
{
    public interface IFileValidator
    {
        FileValidationResult Validate(IFormFileCollection files, FileValidationRuleSet rules);
    }
}
