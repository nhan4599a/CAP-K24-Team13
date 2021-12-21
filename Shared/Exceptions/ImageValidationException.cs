using Shared.Validations;
using System;

namespace Shared.Exceptions
{
    public class ImageValidationException : Exception
    {
        public FileValidationResult ValidationResult { get; set; }

        public ImageValidationException(FileValidationResult validationResult)
        {
            if (!validationResult.IsError)
                throw new ArgumentException("ValidationResult is succeed");
            ValidationResult = validationResult;
        }
    }
}
