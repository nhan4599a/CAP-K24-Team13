using DatabaseAccessor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
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

    }
}
