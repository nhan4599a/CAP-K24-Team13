using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/products")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task AddProduct(int CategoryId, string ProductName, string Description, int Quantity, double Price, int Discount)
        {
            _dbContext.ShopProducts.Add(new ShopProduct
            {
                CategoryId = CategoryId,
                ProductName = ProductName,
                Description = Description,
                Quantity = Quantity,
                Price = Price,
                Discount = Discount,
            });

            await _dbContext.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<ApiResult<PaginatedDataList<ProductDTO>>> ListProduct([FromQuery] int pageNumber, int pageSize = 5)
        {
            var allProducts = await _dbContext.ShopProducts
                                .Select(product => ProductDTO.FromSource(product))
                                .Cast<ProductDTO>()
                                .ToListAsync();
            return new ApiResult<PaginatedDataList<ProductDTO>> { ResponseCode = 200, Data = allProducts.Paginate(pageNumber, pageSize) };
        }
    }
}