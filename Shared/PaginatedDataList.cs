using System.Collections.Generic;

namespace Shared
{
    public class PaginatedDataList<T>
    {
        private int _maxPageNumber;

        private List<T> _data;

        private int _currentPageNumber;

        public int MaxPageNumber { get => _maxPageNumber; }

        public List<T> Data { get => _data; }

        public int CurrentPageNumber { get => _currentPageNumber; }

        public PaginatedDataList(List<T> data, int maxPageNumber, int currentPageNumber)
        {
            _maxPageNumber = maxPageNumber;
            _data = data;
            _currentPageNumber = currentPageNumber;
        }
    }
}
