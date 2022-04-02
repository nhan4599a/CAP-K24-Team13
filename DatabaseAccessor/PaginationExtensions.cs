using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccessor
{
    public static class PaginationExtensions
    {
        public static async Task<PaginatedList<T>> PaginateAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            if (count == 0)
                return PaginatedList<T>.Empty;
            if (pageSize <= 0)
                return PaginatedList<T>.All(await source.ToListAsync());
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, pageNumber, pageSize, count);
        }

        public static PaginatedList<T> Paginate<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            if (count == 0)
                return PaginatedList<T>.Empty;
            if (pageSize <= 0)
                return PaginatedList<T>.All(source.ToList());
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, pageNumber, pageSize, count);
        }
    }
}
