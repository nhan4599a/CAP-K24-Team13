using Microsoft.AspNetCore.Http;
using Shared.Validations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AspNetCoreSharedComponent.FileValidations
{
    public static class FileValidationExtensions
    {
        public static FileValidationResult Validate(this IFormFile file, FileValidationRuleSet? rules = null)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (rules == null || rules.IsEmpty)
                rules = FileValidationRuleSet.DefaultSingleValidationRules;
            List<FileValidationRuleName> passedRules = new();
            foreach (var rule in rules)
                if (file.ValidateSingle(rule))
                    passedRules.Add(rule.RuleName);
            if (passedRules.Count == rules.Count)
                return FileValidationResult.CreateSucceedResult(rules);
            return FileValidationResult.CreateFailedResult(rules, passedRules);
        }

        public static bool ValidateSingle(this IFormFile file, FileValidationRule rule)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));
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
            throw new ArgumentException($"{rule.RuleName} is not suitable for single file validation", nameof(rule));
        }

        public static FileValidationResult Validate(this IFormFileCollection files, FileValidationRuleSet? rules = null)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));
            if (files.Count == 0)
                throw new ArgumentException("file collection must not have zero file", nameof(files));
            if (rules == null || rules.Count == 0)
                rules = FileValidationRuleSet.DefaultValidationRules;
            var passedRules = new List<FileValidationRuleName>();
            foreach (FileValidationRule rule in rules)
                if (files.ValidateMultiple(rule))
                    passedRules.Add(rule.RuleName);
            if (passedRules.Count == rules.Count)
                return FileValidationResult.CreateSucceedResult(rules);
            return FileValidationResult.CreateFailedResult(rules, passedRules);
        }

        public static bool ValidateMultiple(this IFormFileCollection files, FileValidationRule rule)
        {
            if (files == null)
                throw new ArgumentNullException(nameof(files));
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));
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
            foreach (var file in files)
                if (!file.ValidateSingle(rule))
                    return false;
            return true;
        }
    }
}
