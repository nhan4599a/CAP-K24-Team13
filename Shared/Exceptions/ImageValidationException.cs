using Shared.Validations;
using System;

namespace Shared.Exceptions
{
    public class FileValidationException : Exception
    {
        public FileValidationResult ValidationResult { get; set; }

        public FileValidationException(FileValidationResult validationResult)
        {
            if (!validationResult.IsViolatedResult)
                throw new ArgumentException("ValidationResult is succeed");
            ValidationResult = validationResult;
        }

        public override string Message => $"Following file validation rules are violated [{ValidationResult}]";
    }
}
