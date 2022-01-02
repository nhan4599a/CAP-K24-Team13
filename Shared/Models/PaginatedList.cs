using System;
using System.Collections.Generic;

namespace Shared.Models
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
                throw new ArgumentException($"{nameof(pageNumber)} must be greater than 0, passed value is {pageNumber}", nameof(pageNumber));
            if (pageSize < 1)
                throw new ArgumentException($"{nameof(pageSize)} must be greater than 0, passed value is {pageSize}", nameof(pageSize));
            Data = data;
            PageNumber = pageNumber;
            MaxPageNumber = count != 0 ? (int)Math.Ceiling((float)count / pageSize) : 1;
        }
    }
}