using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopProductService.RequestModel;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/categories")]
    public class CategoryController : Controller
    {
        private ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpPost]
        [ActionName("Add")]
        public ApiResult<bool> AddCategory(AddCategoryRequestModel requestModel)
        {
            _dbContext.ShopCategories.Add(new ShopCategory
            {
                ShopId = 1,
                CategoryName = requestModel.categoryName,
                Special = requestModel.special
            });
            _dbContext.SaveChangesAsync();
            
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet]
        public async Task<ApiResult<List<CategoryDTO>>> Index()
        {
            var category =
                await _dbContext.ShopCategories.Select(category => CategoryDTO.FromSource(category)).Cast<CategoryDTO>().ToListAsync();
            return new ApiResult<List<CategoryDTO>> { ResponseCode = 200, Data = category };
        }
    }
}