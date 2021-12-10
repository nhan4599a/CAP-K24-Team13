using System.Collections.Generic;
using System.Linq;

namespace Shared.ImageValidations
{
    public static class ImageValidationLinqHelper
    {
        public static IEnumerable<ImageValidationRuleName> Except(this ImageValidationRuleSet validationRules,
            IEnumerable<ImageValidationRuleName> ruleNames)
        {
            return validationRules.Select(validationRule => validationRule.RuleName).Except(ruleNames);
        }
    }
}
