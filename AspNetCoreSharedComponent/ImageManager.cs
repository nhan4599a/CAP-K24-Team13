using Shared;
using Shared.Exceptions;
using Shared.Validations;

namespace AspNetCoreSharedComponent
{
    public class ImageManager : IFileValidator, IStorable
    {
        private readonly IWebHostEnvironment _environment;

        public ImageManager(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string[]> SaveFilesAsync(IFormFileCollection images, bool shouldValidate = true,
            FileValidationRuleSet? rules = null)
        {
            if (shouldValidate)
            {
                var validationResult = Validate(images, rules);
                if (validationResult.IsError)
                    throw new ImageValidationException(validationResult);
            }
            List<string> savedFileNames = new(5);
            foreach (IFormFile image in images)
                savedFileNames.Add(await SaveFileAsync(image));
            return savedFileNames.ToArray();
        }

        public async Task<string[]> EditFilesAsync(string[] oldImagesName, IFormFileCollection images,
            bool shouldValidate = true, FileValidationRuleSet? rules = null)
        {
            if (shouldValidate)
            {
                var validationResult = Validate(images, rules);
                if (validationResult.IsError)
                    throw new ImageValidationException(validationResult);
            }
            var imageFilesName = images.Select(x => x.FileName).ToList();
            List<string> savedFileNames = new(images.Count);
            var shouldBeEdittedFileNames = oldImagesName.Intersect(imageFilesName).ToList();
            var shouldBeDeletedFileNames = oldImagesName.Except(imageFilesName).ToList();
            var shouldBeCreatedFileNames = imageFilesName.Except(oldImagesName).ToList();
            foreach (var shouldBeEdittedFileName in shouldBeEdittedFileNames)
            {
                await EditFileAsync(
                    GetSavePathForImage(shouldBeEdittedFileName),
                    images.First(image => image.FileName == shouldBeEdittedFileName)
                );
                savedFileNames.Add(shouldBeEdittedFileName);
            }
            foreach (var shouldBeDeletedFileName in shouldBeDeletedFileNames)
                File.Delete(GetSavePathForImage(shouldBeDeletedFileName));
            foreach (var shouldBeCreatedFileName in shouldBeCreatedFileNames)
                savedFileNames.Add(
                    await SaveFileAsync(images.First(image => image.FileName == shouldBeCreatedFileName))
                );
            return savedFileNames.ToArray();
        }

        private async Task<string> SaveFileAsync(IFormFile image, string? fileName = null)
        {
            fileName ??= Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            using var fileStream = File.Create(GetSavePathForImage(fileName));
            await image.CopyToAsync(fileStream);
            return fileName;
        }

        private static async Task EditFileAsync(string filePath, IFormFile image)
        {
            using var fileStream = File.Open(filePath, FileMode.Truncate);
            await image.CopyToAsync(fileStream);
        }

        private string GetSavePathForImage(string imageName) => Path.Combine(_environment.WebRootPath, imageName);

        public FileValidationResult Validate(IFormFileCollection files, FileValidationRuleSet? rules)
        {
            if (rules == null || rules.IsEmpty)
                rules = FileValidationRuleSet.DefaultValidationRules;

            var passedRules = new List<FileValidationRuleName>();
            var suitableValidationRuleForMultiple =
                rules.Where(validationRule => IsSuitableForValidateMultiple(validationRule.RuleName));

            foreach (FileValidationRule rule in suitableValidationRuleForMultiple)
                if (ValidateMultiple(files, rule))
                    passedRules.Add(rule.RuleName);

            foreach (FileValidationRule rule in rules.Except(suitableValidationRuleForMultiple))
                foreach (IFormFile image in files)
                    if (ValidateSingle(image, rule))
                        passedRules.Add(rule.RuleName);

            return new FileValidationResult(rules, passedRules);
        }

        public FileResponse GetImage(string imageName)
        {
            return new FileResponse
            {
                FullPath = GetSavePathForImage(imageName),
                MimeType = string.Concat("image/", Path.GetExtension(imageName).AsSpan(1))
            };
        }

        private static bool ValidateSingle(IFormFile file, FileValidationRule rule)
        {
            if (rule.RuleName == FileValidationRuleName.ImageExtension)
            {
                string fileExtension = Path.GetExtension(file.FileName)[1..];
                if (FileValidationRule.IMAGE_EXTENSIONS.Contains(fileExtension))
                    return true;
                return false;
            }
            if (rule.RuleName == FileValidationRuleName.SingleMaxFileSize)
            {
                if (file.Length <= rule.Value)
                    return true;
                return false;
            }
            return true;
        }

        private static bool ValidateMultiple(IFormFileCollection files, FileValidationRule rule)
        {
            if (rule.RuleName == FileValidationRuleName.MinFileCount)
            {
                if (files.Count >= rule.Value)
                    return true;
                return false;
            }
            if (rule.RuleName == FileValidationRuleName.MaxFileCount)
            {
                if (files.Count <= rule.Value)
                    return true;
                return false;
            }
            if (rule.RuleName == FileValidationRuleName.AllMaxFileSize)
            {
                if (files.Sum(file => file.Length) <= rule.Value)
                    return true;
                return false;
            }
            return true;
        }

        private static bool IsSuitableForValidateMultiple(FileValidationRuleName ruleName)
        {
            return ruleName == FileValidationRuleName.MinFileCount ||
                ruleName == FileValidationRuleName.MinFileCount ||
                ruleName == FileValidationRuleName.AllMaxFileSize;
        }
    }
}