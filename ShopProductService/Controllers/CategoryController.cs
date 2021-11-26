using DatabaseAccessor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Threading.Tasks;
using Shared.DTOs;
using System.Collections.Generic;
using System.Linq;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoryController : Controller
    {
        private ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpPut]
        [ActionName("Edit")]
        public async Task<ApiResult<bool>> EditCategory(int id, string CategoryName, int Special)
        {
            var category = await _dbContext.ShopCategories.FirstOrDefaultAsync(ct => ct.Id == id);
            if (category == null)
            {
                return new ApiResult<bool> { ResponseCode = 404, ErrorMessage = "Product not found", Data = false };
            }
            category.CategoryName = CategoryName;
            category.Special = Special;
            var result = await _dbContext.SaveChangesAsync() > 0;
            if (!result)
            {
                return new ApiResult<bool> { ResponseCode = 500, Data = false };
            }
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet]
        public async Task<ApiResult<PaginatedDataList<CategoryDTO>>> ListCategory([FromQuery] int pageNumber, int pageSize = 5)
        {
            var categories = await _dbContext.ShopCategories
                                    .Select(category => CategoryDTO.FromSource(category))
                                    .Cast<CategoryDTO>()
                                    .ToListAsync();
            return new ApiResult<PaginatedDataList<CategoryDTO>> { ResponseCode = 200, Data = categories.Paginate(pageNumber, pageSize) };
        }

        [HttpDelete]
        [ActionName("Delete")]
        public async Task<ApiResult<bool>> DeleteCategory([FromBody] int categoryId)
        {
            var category = await _dbContext.ShopCategories.FindAsync(categoryId);
            if (category == null)
                return new ApiResult<bool> { ResponseCode = 404, ErrorMessage = "Category not found", Data = false };
            _dbContext.ShopCategories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }
    }
}
