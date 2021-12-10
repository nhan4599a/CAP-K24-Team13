namespace Shared.ImageValidations
{
    public class ImageValidationRule
    {
        public static readonly string[] IMAGE_EXTENSIONS = { "jpg", "jpeg", "png" };
        public ImageValidationRuleName RuleName { get; set; }

        public long Value { get; set; }
    }
}
