using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class LinqHelper
    {
        public static PaginatedDataList<T> Paginate<T>(this List<T> dataList, int pageNumber, int pageSize)
        {
            var paginatedData = dataList
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
            var maxPageNumber = (int)System.Math.Ceiling((float)dataList.Count / pageSize);
            return new PaginatedDataList<T>(paginatedData, maxPageNumber, pageNumber);
        }
    }
}
