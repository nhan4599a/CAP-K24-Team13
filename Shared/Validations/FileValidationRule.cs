using System.Collections.Generic;

namespace Shared.Validations
{
    public class FileValidationRule
    {
        public static readonly List<string> IMAGE_EXTENSIONS = new () { "jpg", "jpeg", "png" };

        public FileValidationRuleName RuleName { get; set; }

        public long Value { get; set; }

        public override int GetHashCode()
        {
            return RuleName.GetHashCode();
        }
    }
}
