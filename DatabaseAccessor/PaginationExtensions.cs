using Microsoft.EntityFrameworkCore;
using Shared;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccessor
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedList<T>> PaginateAsync<T>(this IQueryable<T> source, int? pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            if (!pageNumber.HasValue)
                return new PaginatedList<T>(await source.ToListAsync(), 1, count, count);
            var items = await source.Skip((pageNumber.Value - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, pageNumber.Value, pageSize, count);
        }
    }
}
