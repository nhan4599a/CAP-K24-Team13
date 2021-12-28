using System;
using System.Collections.Generic;

namespace Shared
{
    public class PaginatedList<T>
    {
        public int PageNumber { get; private set; }

        public int MaxPageNumber { get; private set; }

        public bool HasPreviousPage
        {
            get { return PageNumber > 1; }
        }

        public bool HasNextPage
        {
            get { return PageNumber < MaxPageNumber; }
        }

        public List<T> Data { get; private set; }

        public PaginatedList(List<T> data, int pageNumber, int pageSize, int count)
        {
            if (pageNumber < 1)
                throw new ArgumentException($"{pageNumber} must be greater than 0", nameof(pageNumber));
            if (pageSize < 1)
                throw new ArgumentException($"{pageSize} must be greater than 0", nameof(pageSize));
            Data = data;
            PageNumber = pageNumber;
            MaxPageNumber = (int)Math.Ceiling((float)count / pageSize);
        }
    }
}