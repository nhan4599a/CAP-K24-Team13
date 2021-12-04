using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using ShopProductService.RequestModel;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<ApiResult<bool>> AddCategory(AddOrEditCategoryRequestModel requestModel)
        {
            _dbContext.ShopCategories.Add(new ShopCategory
            {
                ShopId = 1,
                CategoryName = requestModel.CategoryName,
                Special = requestModel.Special
            });
            await _dbContext.SaveChangesAsync();
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet("id")]
        public async Task<ApiResult<CategoryDTO>> DetailCategory(int id)
        {
            var category = await _dbContext.ShopCategories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return new ApiResult<CategoryDTO> { ResponseCode = 404, Data = null };
            var categoryDto = new CategoryDTO
            {
                CategoryName = category.CategoryName,
                Id = category.Id
            };
            return new ApiResult<CategoryDTO> { ResponseCode = 200, Data = categoryDto };
        }

        [HttpPut("id")]
        public async Task<ApiResult<bool>> EditCategory(int id, AddOrEditCategoryRequestModel requestModel)
        {
            var category = await _dbContext.ShopCategories.FindAsync(id);
            if (category == null)
            {
                return new ApiResult<bool> { ResponseCode = 404, ErrorMessage = "Category not found", Data = false };
            }
            category.CategoryName = requestModel.CategoryName;
            category.Special = requestModel.Special;
            var result = await _dbContext.SaveChangesAsync() > 0;
            if (!result)
            {
                return new ApiResult<bool> { ResponseCode = 500, Data = false };
            }
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }
        
        [HttpGet]
        [ActionName("Index")]
        public async Task<ApiResult<PaginatedDataList<CategoryDTO>>> ListCategory([FromQuery] PaginationInfo paginationInfo)
        {
            var categories = await _dbContext.ShopCategories.AsNoTracking()
                                    .Select(category => CategoryDTO.FromSource(category))
                                    .Cast<CategoryDTO>()
                                    .ToListAsync();
            return new ApiResult<PaginatedDataList<CategoryDTO>> 
            {
                ResponseCode = 200,
                Data = categories.Paginate(paginationInfo.PageNumber, paginationInfo.PageSize) 
            };
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
