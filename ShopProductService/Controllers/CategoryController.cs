using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public async Task AddCategory([FromBody] ShopCategory category)
        {
            _dbContext.ShopCategories.Add(category);

            await _dbContext.SaveChangesAsync();
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