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

        public ImageValidationResult Validate(IFormFileCollection images)
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

        public async Task<string[]> SaveFileAsync(IFormFileCollection images)
        {
            var validationResult = Validate(images);
            if (validationResult.IsError)
                throw new ImageValidationException(validationResult);
            List<string> saveFileNames = new(5);
            for (int i = 0; i < images.Count; i++)
            {
                var image = images[i];
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var savePath = Path.Combine(_environment.WebRootPath, fileName);
                using (var fileStream = File.Create(savePath))
                {
                    await image.CopyToAsync(fileStream);
                }
                saveFileNames.Add(fileName);
            }
            return saveFileNames.ToArray();
        }

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
            var fullPath = Path.Combine(_environment.WebRootPath, imageName);
            return new FileResponse
            {
                FullPath = fullPath,
                MimeType = string.Concat("image/", Path.GetExtension(imageName).AsSpan(1))
            };
        }

        private static bool ValidateSingle(IFormFile file, ImageValidationRule rule)
        {
            if (rule.RuleName == ImageValidationRuleName.ImageExtension)
            {
                var fileExtension = Path.GetExtension(file.FileName).Substring(1);
                if (Array.Exists(ImageValidationRule.IMAGE_EXTENSIONS, extension => extension == fileExtension))
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
