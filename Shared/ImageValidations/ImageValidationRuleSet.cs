using System.Collections;
using System.Collections.Generic;

namespace Shared.ImageValidations
{
    public class ImageValidationRuleSet : IList<ImageValidationRule>
    {
        private readonly IList<ImageValidationRule> _rules;

        public ImageValidationRuleSet()
        {
            _rules = new List<ImageValidationRule>();
        }

        public ImageValidationRuleSet(List<ImageValidationRule> rules)
        {
            _rules = rules;
        }

        public ImageValidationRule this[int index] => _rules[index];

        ImageValidationRule IList<ImageValidationRule>.this[int index] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public int Count => _rules.Count;

        public bool IsReadOnly => throw new System.NotImplementedException();

        public void Add(ImageValidationRule item)
        {
            _rules.Add(item);
        }

        public void Clear()
        {
            _rules.Clear();
        }

        public bool Contains(ImageValidationRule item) => _rules.Contains(item);

        public void CopyTo(ImageValidationRule[] array, int arrayIndex)
        {
            _rules.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ImageValidationRule> GetEnumerator() => _rules.GetEnumerator();

        public int IndexOf(ImageValidationRule item) => _rules.IndexOf(item);

        public void Insert(int index, ImageValidationRule item)
        {
            _rules.Insert(index, item);
        }

        public bool Remove(ImageValidationRule item) => _rules.Remove(item);

        public void RemoveAt(int index)
        {
            _rules.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator() => _rules.GetEnumerator();
    }
}
