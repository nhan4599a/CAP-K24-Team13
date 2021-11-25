using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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
        public async Task<ApiResult<bool>> AddCategory(int ShopId, string CategoryName, int Special)
        {
            _dbContext.ShopCategories.Add(new ShopCategory
            {
                ShopId = ShopId,
                CategoryName = CategoryName,
                Special = Special,
            });
            await _dbContext.SaveChangesAsync();
            
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet]
        public async Task<ApiResult<List<CategoryDTO>>> Index()
        {
            var category = _dbContext.ShopCategories.Select(category => CategoryDTO.FromSource(category)).Cast<CategoryDTO>().ToList();
            return new ApiResult<List<CategoryDTO>> { ResponseCode = 200, Data = category };
        }
    }
}