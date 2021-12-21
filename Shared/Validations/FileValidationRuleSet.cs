using System.Collections;
using System.Collections.Generic;

namespace Shared.Validations
{
    public class FileValidationRuleSet : IList<FileValidationRule>
    {
        private readonly IList<FileValidationRule> _rules;

        public FileValidationRuleSet()
        {
            _rules = new List<FileValidationRule>();
        }

        public FileValidationRuleSet(List<FileValidationRule> rules)
        {
            _rules = rules;
        }

        public FileValidationRule this[int index] => _rules[index];

        FileValidationRule IList<FileValidationRule>.this[int index] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public int Count => _rules.Count;

        public bool IsReadOnly => throw new System.NotImplementedException();

        public void Add(FileValidationRule item)
        {
            _rules.Add(item);
        }

        public void Clear()
        {
            _rules.Clear();
        }

        public bool Contains(FileValidationRule item) => _rules.Contains(item);

        public void CopyTo(FileValidationRule[] array, int arrayIndex)
        {
            _rules.CopyTo(array, arrayIndex);
        }

        public IEnumerator<FileValidationRule> GetEnumerator() => _rules.GetEnumerator();

        public int IndexOf(FileValidationRule item) => _rules.IndexOf(item);

        public void Insert(int index, FileValidationRule item)
        {
            _rules.Insert(index, item);
        }

        public bool Remove(FileValidationRule item) => _rules.Remove(item);

        public void RemoveAt(int index)
        {
            _rules.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() => _rules.GetEnumerator();
    }
}
