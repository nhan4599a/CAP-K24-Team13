using System.Collections.Generic;
using System.Linq;

namespace Shared.Validations
{
    public class FileValidationResult
    {
        public List<FileValidationRuleName> ViolatedRules { get; set; }

        public List<FileValidationRuleName> PassedRules { get; set; }

        public bool IsError => ViolatedRules.Count > 0;

        public FileValidationResult(FileValidationRuleSet rules, List<FileValidationRuleName> passedRules)
        {
            PassedRules = passedRules;
            ViolatedRules = rules.Except(passedRules).ToList();
        }
    }
}
