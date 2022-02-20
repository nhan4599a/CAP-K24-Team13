using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Validations
{
    public class FileValidationResult
    {
        public IEnumerable<FileValidationRuleName> ViolatedRules { get; }

        public IEnumerable<FileValidationRuleName> PassedRules { get; }

        public bool IsViolatedResult => ViolatedRules.Any();

        private FileValidationResult(FileValidationRuleSet rules, IEnumerable<FileValidationRuleName> passedRules)
        {
            PassedRules = passedRules;
            if (passedRules.Any(ruleName => !rules.Contains(ruleName)))
                throw new ArgumentException($"{nameof(passedRules)} is contains a value that not exist in provided ${nameof(rules)}");
            ViolatedRules = rules.Except(passedRules).ToList();
        }

        public static FileValidationResult CreateSucceedResult(FileValidationRuleSet rules)
        {
            if (rules == null)
                throw new ArgumentNullException(nameof(rules));
            return new FileValidationResult(rules, rules.Select(rule => rule.RuleName));
        }

        public static FileValidationResult CreateFailedResult(FileValidationRuleSet rules, IEnumerable<FileValidationRuleName> passedRules)
        {
            if (rules == null)
                throw new ArgumentNullException(nameof(rules));
            if (passedRules == null)
                throw new ArgumentNullException(nameof(passedRules));
            if (!rules.Except(passedRules).Any())
                throw new InvalidOperationException($"{nameof(rules)} and {nameof(passedRules)} may not seem like a failed validation resule");
            return new FileValidationResult(rules, passedRules);
        }

        public override string ToString()
        {
            return string.Join(", ", ViolatedRules);
        }
    }
}
