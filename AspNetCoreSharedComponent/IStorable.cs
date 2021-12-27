using Shared.Validations;

namespace AspNetCoreSharedComponent
{
    public interface IStorable
    {
        Task<string[]> SaveFilesAsync(IFormFileCollection files, bool shouldValidate = true,
            FileValidationRuleSet? rules = null);

        Task<string[]> EditFilesAsync(string[] oldFilesName, IFormFileCollection files, bool shoulValidate = true,
            FileValidationRuleSet? rules = null);
    }
}
