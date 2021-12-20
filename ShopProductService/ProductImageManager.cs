using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Shared;
using Shared.Exceptions;
using Shared.ImageValidations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProductService
{
    public class ProductImageManager
    {
        private readonly IWebHostEnvironment _environment;

        public ProductImageManager(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public virtual ImageValidationResult Validate(IFormFileCollection images)
        {
            var validationRules = new ImageValidationRuleSet
            {
                new ImageValidationRule
                {
                    RuleName = ImageValidationRuleName.ImageExtension,
                },
                new ImageValidationRule
                {
                    RuleName = ImageValidationRuleName.MinFileCount,
                    Value = 2
                },
                new ImageValidationRule
                {
                    RuleName = ImageValidationRuleName.MaxFileCount,
                    Value = 5
                },
                new ImageValidationRule
                {
                    RuleName = ImageValidationRuleName.SingleMaxFileSize,
                    Value = 1024 * 1024
                },
                new ImageValidationRule
                {
                    RuleName = ImageValidationRuleName.AllMaxFileSize,
                    Value = 4 * 1024 * 1024
                }
            };
            return Validate(images, validationRules);
        }

        public virtual async Task<string[]> SaveFilesAsync(IFormFileCollection images)
        {
            var validationResult = Validate(images);
            if (validationResult.IsError)
                throw new ImageValidationException(validationResult);
            List<string> savedFileNames = new(5);
            foreach (IFormFile image in images)
                savedFileNames.Add(await SaveFileAsync(image));
            return savedFileNames.ToArray();
        }

        public virtual async Task<string[]> EditFilesAsync(string[] oldImagesName, IFormFileCollection images)
        {
            var validationResult = Validate(images);
            if (validationResult.IsError)
                throw new ImageValidationException(validationResult);
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

        private async Task<string> SaveFileAsync(IFormFile image, string fileName = null)
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

        public ImageValidationResult Validate(IFormFileCollection images, ImageValidationRuleSet validationRules)
        {
            var passedRules = new List<ImageValidationRuleName>();
            var suitableValidationRuleForMultiple =
                validationRules.Where(validationRule => IsSuitableForValidateMultiple(validationRule.RuleName));

            foreach (ImageValidationRule rule in suitableValidationRuleForMultiple)
                if (ValidateMultiple(images, rule))
                    passedRules.Add(rule.RuleName);

            foreach (ImageValidationRule rule in validationRules.Except(suitableValidationRuleForMultiple))
                foreach (IFormFile image in images)
                    if (ValidateSingle(image, rule))
                        passedRules.Add(rule.RuleName);

            return new ImageValidationResult(validationRules, passedRules);
        }

        public FileResponse GetImage(string imageName)
        {
            return new FileResponse
            {
                FullPath = GetSavePathForImage(imageName),
                MimeType = string.Concat("image/", Path.GetExtension(imageName).AsSpan(1))
            };
        }

        private static bool ValidateSingle(IFormFile file, ImageValidationRule rule)
        {
            if (rule.RuleName == ImageValidationRuleName.ImageExtension)
            {
                string fileExtension = Path.GetExtension(file.FileName)[1..];
                if (ImageValidationRule.IMAGE_EXTENSIONS.Contains(fileExtension))
                    return true;
                return false;
            }
            if (rule.RuleName == ImageValidationRuleName.SingleMaxFileSize)
            {
                if (file.Length <= rule.Value)
                    return true;
                return false;
            }
            return true;
        }

        private static bool ValidateMultiple(IFormFileCollection files, ImageValidationRule rule)
        {
            if (rule.RuleName == ImageValidationRuleName.MinFileCount)
            {
                if (files.Count >= rule.Value)
                    return true;
                return false;
            }
            if (rule.RuleName == ImageValidationRuleName.MaxFileCount)
            {
                if (files.Count <= rule.Value)
                    return true;
                return false;
            }
            if (rule.RuleName == ImageValidationRuleName.AllMaxFileSize)
            {
                if (files.Sum(file => file.Length) <= rule.Value)
                    return true;
                return false;
            }
            return true;
        }

        private static bool IsSuitableForValidateMultiple(ImageValidationRuleName ruleName)
        {
            return ruleName == ImageValidationRuleName.MinFileCount ||
                ruleName == ImageValidationRuleName.MinFileCount ||
                ruleName == ImageValidationRuleName.AllMaxFileSize;
        }
    }
}
