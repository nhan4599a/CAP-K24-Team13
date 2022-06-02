using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Validations
{
    public class FileValidationRuleSet : IReadOnlySet<FileValidationRule>
    {
        private readonly ISet<FileValidationRule> _rules;

        public static FileValidationRuleSet DefaultValidationRules
        {
            get => new()
            {
                new FileValidationRule
                {
                    RuleName = FileValidationRuleName.ImageExtension,
                },
                new FileValidationRule
                {
                    RuleName = FileValidationRuleName.MinFileCount,
                    Value = 2
                },
                new FileValidationRule
                {
                    RuleName = FileValidationRuleName.MaxFileCount,
                    Value = 5
                },
                new FileValidationRule
                {
                    RuleName = FileValidationRuleName.SingleMaxFileSize,
                    Value = 1024 * 1024
                },
                new FileValidationRule
                {
                    RuleName = FileValidationRuleName.AllMaxFileSize,
                    Value = 4 * 1024 * 1024
                }
            };
        }

        public static FileValidationRuleSet DefaultSingleValidationRules
        {
            get => new()
            {
                new FileValidationRule
                {
                    RuleName = FileValidationRuleName.ImageExtension,
                },
                new FileValidationRule
                {
                    RuleName = FileValidationRuleName.SingleMaxFileSize,
                    Value = 1024 * 1024
                }
            };
        }

        public int Count => _rules.Count;

        public bool IsEmpty => _rules.Count == 0;

        public FileValidationRule this[FileValidationRuleName ruleName]
        {
            get => _rules.FirstOrDefault(rule => rule.RuleName == ruleName);
            set => Change(ruleName, value.Value);
        }

        public FileValidationRuleSet()
        {
            _rules = new HashSet<FileValidationRule>();
        }

        public FileValidationRuleSet(ISet<FileValidationRule> rules) : this()
        {
            foreach (FileValidationRule rule in rules)
                _rules.Add(rule);
        }

        public void Add(FileValidationRule rule)
        {
            if (Contains(rule))
                throw new InvalidOperationException($"{rule.RuleName} rule is already exsited");
            _rules.Add(rule);
        }

        public void Change(FileValidationRuleName ruleName, long value)
        {
            if (!Contains(this[ruleName]))
                throw new InvalidOperationException($"{ruleName} rule is not existed");
            _rules.Remove(this[ruleName]);
            _rules.Add(new FileValidationRule
            {
                RuleName = ruleName,
                Value = value
            });
        }

        public void Remove(FileValidationRuleName ruleName)
        {
            var rule = this[ruleName];
            if (rule == null)
                throw new InvalidOperationException($"{ruleName} rule is not existed");
            _rules.Remove(rule);
        }

        public bool Contains(FileValidationRule item) => item != null && this.Contains(item.RuleName);

        public bool IsProperSubsetOf(IEnumerable<FileValidationRule> other) => _rules.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<FileValidationRule> other) => _rules.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<FileValidationRule> other) => _rules.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<FileValidationRule> other) => _rules.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<FileValidationRule> other) => _rules.Overlaps(other);

        public bool SetEquals(IEnumerable<FileValidationRule> other) => _rules.SetEquals(other);

        public IEnumerator<FileValidationRule> GetEnumerator() => _rules.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
