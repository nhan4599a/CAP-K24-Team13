using System.Collections.Generic;

namespace Shared.ImageValidations
{
    public class ImageValidationRule
    {
        public static readonly List<string> IMAGE_EXTENSIONS = new () { "jpg", "jpeg", "png" };
        public ImageValidationRuleName RuleName { get; set; }

        public long Value { get; set; }
    }
}
