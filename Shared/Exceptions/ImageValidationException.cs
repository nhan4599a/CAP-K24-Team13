using Shared.ImageValidations;
using System;

namespace Shared.Exceptions
{
    public class ImageValidationException : Exception
    {
        public ImageValidationResult ValidationResult { get; set; }

        public ImageValidationException(ImageValidationResult validationResult)
        {
            if (!validationResult.IsError)
                throw new ArgumentException();
            ValidationResult = validationResult;
        }
    }
}
