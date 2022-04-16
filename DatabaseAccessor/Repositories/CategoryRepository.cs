using DatabaseAccessor.Contexts;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Models;
using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<CategoryDTO>> GetCategoriesOfShopAsync(int shopId, PaginationInfo paginationInfo)
        {
            return await _dbContext.Categories
                .FromSqlRaw("SELECT CategoryId, Category as CategoryName, COUNT(*) as ProductCoung FROM dbo.ShopProducts " +
                    "WHERE ShopId = @ShopId GROUP BY ShopId, CategoryId, Category", new SqlParameter("@ShopId", shopId))
                .AsNoTracking()
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
