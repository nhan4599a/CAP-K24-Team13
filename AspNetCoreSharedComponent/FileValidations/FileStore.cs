using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Shared.Models;
using Shared.Validations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSharedComponent.FileValidations
{
    public class FileStore : IFileStorable
    {
        private readonly IWebHostEnvironment _environment;

        private string _relationalPath = "";

        private DirectoryInfo _directoryInfo;

        public FileStore(IWebHostEnvironment environment)
        {
            _environment = environment;
            _directoryInfo = new DirectoryInfo(Path.Combine(_environment.WebRootPath, _relationalPath));
        }

        public void SetRelationalPath(string relationalPath)
        {
            _relationalPath = relationalPath;
            _directoryInfo = new DirectoryInfo(Path.Combine(_environment.WebRootPath, _relationalPath));
        }

        public async Task<string[]> SaveFilesAsync(IEnumerable<IFormFile> files, bool shouldValidate = true,
            FileValidationRuleSet? rules = null)
        {
            if (shouldValidate)
            {
                var validationResult = files.Validate(rules);
                if (validationResult.IsViolatedResult)
                    throw new ImageValidationException(validationResult);
            }
            CreateDirectory();
            List<string> savedFileNames = new(5);
            foreach (IFormFile image in files)
                savedFileNames.Add(await SaveFileAsync(image, null));
            return savedFileNames.ToArray();
        }

        public async Task<string[]> EditFilesAsync(string[] oldImagesName, IEnumerable<IFormFile> files,
            bool shouldValidate = true, FileValidationRuleSet? rules = null)
        {
            if (oldImagesName == null)
                throw new ArgumentNullException(nameof(oldImagesName));
            if (oldImagesName.Length == 0)
                return await SaveFilesAsync(files, shouldValidate, rules);
            if (shouldValidate)
            {
                var validationResult = files.Validate(rules);
                if (validationResult.IsViolatedResult)
                    throw new ImageValidationException(validationResult);
            }
            CreateDirectory();
            var imageFilesName = files.Select(x => x.FileName).ToList();
            List<string> savedFileNames = new(files.Count());
            var shouldBeEdittedFileNames = oldImagesName.Intersect(imageFilesName).ToList();
            var shouldBeDeletedFileNames = oldImagesName.Except(imageFilesName).ToList();
            var shouldBeCreatedFileNames = imageFilesName.Except(oldImagesName).ToList();
            foreach (var shouldBeEdittedFileName in shouldBeEdittedFileNames)
            {
                savedFileNames.Add(
                    await SaveFileAsync(files.First(image => image.FileName == shouldBeEdittedFileName), shouldBeEdittedFileName)
                );
            }
            foreach (var shouldBeDeletedFileName in shouldBeDeletedFileNames)
                File.Delete(GetSavePathForFile(shouldBeDeletedFileName));
            foreach (var shouldBeCreatedFileName in shouldBeCreatedFileNames)
                savedFileNames.Add(
                    await SaveFileAsync(files.First(image => image.FileName == shouldBeCreatedFileName), null)
                );
            return savedFileNames.ToArray();
        }

        public async Task<string> SaveFileAsync(IFormFile file,
            bool shouldValidate = true, FileValidationRuleSet? rules = null)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (shouldValidate)
            {
                var validationResult = file.Validate(rules);
                if (validationResult.IsViolatedResult)
                    throw new ImageValidationException(validationResult);
            }
            CreateDirectory();
            return await SaveFileAsync(file, null);
        }

        public async Task<string> EditFileAsync(string oldFileName, IFormFile file, bool shouldValidate = true,
            FileValidationRuleSet? rules = null)
        {
            if (oldFileName == null)
                throw new ArgumentNullException(nameof(oldFileName));
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            if (shouldValidate)
            {
                var validationResult = file.Validate(rules);
                if (validationResult.IsViolatedResult)
                    throw new ImageValidationException(validationResult);
            }
            CreateDirectory();
            return await SaveFileAsync(file, oldFileName);
        }

        private async Task<string> SaveFileAsync(IFormFile file, string? fileName = null)
        {
            fileName ??= Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var savePath = GetSavePathForFile(fileName);
            FileStream fileStream;
            if (File.Exists(savePath))
                fileStream = File.Open(savePath, FileMode.Truncate);
            else
                fileStream = File.Create(savePath);
            using (fileStream)
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }

        public string GetSavePathForFile(string fileName) => Path.Combine(_directoryInfo.FullName, fileName);

        public FileResponse GetFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(fileName);
            return new FileResponse
            {
                FullPath = GetSavePathForFile(fileName),
                MimeType = string.Concat("image/", Path.GetExtension(fileName).AsSpan(1))
            };
        }

        private void CreateDirectory()
        {
            if (!_directoryInfo.Exists)
                _directoryInfo.Create();
        }
    }
}