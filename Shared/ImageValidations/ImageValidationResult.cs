using System.Collections.Generic;
using System.Linq;

namespace Shared.ImageValidations
{
    public class ImageValidationResult
    {
        public List<ImageValidationRuleName> ViolatedRules { get; set; }

        public List<ImageValidationRuleName> PassedRules { get; set; }

        public bool IsError => ViolatedRules.Count > 0;

        public ImageValidationResult(ImageValidationRuleSet validationRules, List<ImageValidationRuleName> passedRules)
        {
            PassedRules = passedRules;
            ViolatedRules = validationRules.Except(passedRules).ToList();
        }
    }
}
