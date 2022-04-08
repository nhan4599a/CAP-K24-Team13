using Microsoft.AspNetCore.Http;
using Shared.Models;
using Shared.Validations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreSharedComponent.FileValidations
{
    public interface IFileStorable
    {
        Task<string[]> SaveFilesAsync(IEnumerable<IFormFile> files, bool shouldValidate = true, FileValidationRuleSet? rules = null);

        Task<string[]> EditFilesAsync(string[] oldFileName, IEnumerable<IFormFile> files, bool shouldValidate = true,
            FileValidationRuleSet? rules = null);

        Task<string> SaveFileAsync(IFormFile file, bool shouldValidate = true, FileValidationRuleSet? rules = null);

        Task<string> EditFileAsync(string oldFileName, IFormFile file, bool shouldValidate = true, FileValidationRuleSet? rules = null);

        FileResponse GetFile(string fileName);

        string GetSavePathForFile(string fileName);

        void SetRelationalPath(string relationalPath);
    }
}