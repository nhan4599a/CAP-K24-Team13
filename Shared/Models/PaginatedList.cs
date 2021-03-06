using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class PaginatedList<T>
    {
        public int PageNumber { get; private set; }

        public int MaxPageNumber { get; private set; }

        public bool HasPreviousPage
        {
            get => PageNumber > 1;
        }

        public bool HasNextPage
        {
            get => PageNumber < MaxPageNumber;
        }

        public bool IsEmpty
        {
            get => Data.Count == 0;
        }

        public IReadOnlyList<T> Data { get; private set; }

        [JsonConstructor]
        public PaginatedList(int pageNumber, int maxPageNumber, IReadOnlyList<T> data) 
        {
            PageNumber = pageNumber;
            MaxPageNumber = maxPageNumber;
            Data = data;
        }

        public PaginatedList(List<T> data, int pageNumber, int? pageSize, int count)
        {
            if (pageNumber < 1)
                throw new ArgumentException($"{nameof(pageNumber)} must be greater than 0, passed value is {pageNumber}", nameof(pageNumber));
            if (pageSize.HasValue && pageSize < 1)
                throw new ArgumentException($"{nameof(pageSize)} must be greater than 0, passed value is {pageSize}", nameof(pageSize));
            Data = data.AsReadOnly();
            PageNumber = pageNumber;
            MaxPageNumber = count != 0 ? (int)Math.Ceiling((float)count / pageSize.Value) : 1;
        }

        public static PaginatedList<T> Empty => new(new List<T>(), 1, null, 0);

        public static PaginatedList<T> All(List<T> data) => new(data, 1, data.Count, data.Count);

        public IEnumerator<T> GetEnumerator() => Data.GetEnumerator();
	}
}