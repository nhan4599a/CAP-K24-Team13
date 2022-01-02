using Shared;
using Shared.Models;
using Shared.Validations;

namespace AspNetCoreSharedComponent.FileValidations
{
    public interface IFileStorable
    {
        Task<string[]> SaveFilesAsync(IFormFileCollection files, bool shouldValidate = true, FileValidationRuleSet? rules = null);

        Task<string[]> EditFilesAsync(string[] oldFileName, IFormFileCollection files, bool shouldValidate = true,
            FileValidationRuleSet? rules = null);

        Task<string> SaveFileAsync(IFormFile file, bool shouldValidate = true, FileValidationRuleSet? rules = null);

        Task<string> EditFileAsync(string oldFileName, IFormFile file, bool shouldValidate = true, FileValidationRuleSet? rules = null);

        FileResponse GetFile(string fileName);

        string GetSavePathForFile(string fileName);

        void SetRelationalPath(string relationalPath);
    }
}