using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Shared;
using System.Threading.Tasks;
using Shared.DTOs;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
        public ApiResult<List<ProductDTO>> ListProduct([FromQuery] int pageNumber, int pageSize = 5)
        {
            var products = _dbContext.ShopProducts.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Select(product => ProductDTO.FromSource(product)).Cast<ProductDTO>().ToList();
            return new ApiResult<List<ProductDTO>> { ResponseCode = 200, Data = products };
        }
    }
}