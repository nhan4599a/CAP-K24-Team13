using Shared.Models;
using System.Collections.Generic;

namespace GUI.Models
{
    public class ExternalApiPaginatedList<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int PageCount { get; set; }

        public IReadOnlyList<T> Items { get; set; }

        public PaginatedList<T> ToInternal()
        {
            return new PaginatedList<T>(PageIndex, PageSize, PageCount, Items);
        }
    }
}
