using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccessor
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedList<T>> PaginateAsync<T>(this IQueryable<T> source, int pageNumber, int? pageSize)
        {
            var count = await source.CountAsync();
            if (!pageSize.HasValue || count == 0)
                return PaginatedList<T>.Empty;
            var items = await source.Skip((pageNumber - 1) * pageSize.Value).Take(pageSize.Value).ToListAsync();
            return new PaginatedList<T>(items, pageNumber, pageSize.Value, count);
        }
    }
}
