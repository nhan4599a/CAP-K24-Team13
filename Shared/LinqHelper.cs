using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public static class LinqHelper
    {
        public static PaginatedDataList<T> Paginate<T>(this List<T> dataList, int? pageNumber, int pageSize)
        {
            if (!pageNumber.HasValue)
                return new PaginatedDataList<T>(dataList, 1, 1);
            var paginatedData = dataList
                            .Skip((pageNumber.Value - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            var maxPageNumber = (int)System.Math.Ceiling((float)dataList.Count / pageSize);
            return new PaginatedDataList<T>(paginatedData, maxPageNumber, pageNumber.Value);
        }
    }
}
