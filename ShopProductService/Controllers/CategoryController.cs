using DatabaseAccessor;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
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

        public async Task<ApiResult<List<CategoryDTO>>> Index()
        {
            var categories = await _dbContext.ShopCategories.Select(category => CategoryDTO.FromSource(category))
                .Cast<CategoryDTO>().ToListAsync();
            return new ApiResult<List<CategoryDTO>> { ResponseCode = 200, Data = categories };
        }
    }
}
