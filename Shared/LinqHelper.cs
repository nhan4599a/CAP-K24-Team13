using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public static class LinqHelper
    {
        public static PaginatedDataList<T> ToPaginatedDataList<T>(this List<T> dataList, int? pageNumber, int pageSize)
        {
            if (!pageNumber.HasValue)
                return new PaginatedDataList<T>(dataList, 1, 1);
            var maxPageNumber = (int)System.Math.Ceiling((float)dataList.Count / pageSize);
            return new PaginatedDataList<T>(dataList, maxPageNumber, pageNumber.Value);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> list, int? pageNumber, int pageSize)
        {
            if (!pageNumber.HasValue)
                return list;
            return list.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize);
        }
    }
}
