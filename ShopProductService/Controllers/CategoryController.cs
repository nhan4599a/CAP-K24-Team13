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

        [HttpGet]
        public async Task<ApiResult<PaginatedDataList<CategoryDTO>>> ListCategory(int pageNumber, int pageSize = 5)
        {
            var categories = await _dbContext.ShopCategories
                                    .Select(category => CategoryDTO.FromSource(category))
                                    .Cast<CategoryDTO>()
                                    .ToListAsync();
            return new ApiResult<PaginatedDataList<CategoryDTO>> { ResponseCode = 200, Data = categories.Paginate(pageNumber, pageSize) };
        }
    }
}
