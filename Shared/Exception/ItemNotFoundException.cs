using System;

namespace Shared.Exception
{
    public class ItemNotFoundException<TKey, TModel> : SystemException
    {
        public TKey Key { get; private set; }

        public string FullQualifiedClassName { get; private set; }

        public ItemNotFoundException(TKey key)
        {
            Key = key;
            FullQualifiedClassName = typeof(TModel).FullName;
        }
    }
}
