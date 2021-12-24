using System.Collections.Generic;

namespace Shared
{
    public class PaginatedDataList<T>
    {
        private readonly int _maxPageNumber;

        private readonly List<T> _data;

        private readonly int _currentPageNumber;

        public int MaxPageNumber { get => _maxPageNumber; }

        public List<T> Data { get => _data; }

        public int CurrentPageNumber { get => _currentPageNumber; }

        public int Count { get => _data.Count; }

        public T this[int index] => _data[index];

        public PaginatedDataList(List<T> data, int maxPageNumber, int currentPageNumber)
        {
            _maxPageNumber = maxPageNumber;
            _data = data;
            _currentPageNumber = currentPageNumber;
        }
    }
}
