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
        [HttpPut]
        [ActionName("Edit")]
        public async Task<ApiResult<bool>> EditProduct(string ProductId, int CategoryId, string ProductName, string Description, int Quantity, double Price, int Discount)
        {
            var product = await _dbContext.ShopProducts.FirstOrDefaultAsync(p => p.Id == ProductId);
            if (product == null || product.IsDisabled)
                return new ApiResult<bool> { ResponseCode = 404, ErrorMessage = "Product not found", Data = false };
            product.CategoryId = CategoryId;
            product.ProductName = ProductName;
            product.Description = Description;
            product.Quantity = Quantity;
            product.Price = Price;
            product.Discount = Discount;
            bool result = await _dbContext.SaveChangesAsync() > 0;
            if (!result)
            {
                return new ApiResult<bool> { ResponseCode = 500, Data = false };
            }
            return new ApiResult<bool> { ResponseCode = 200, Data = true };

        }
        [HttpDelete]
        [ActionName("Delete")]
        public async Task<ApiResult<bool>> DeleteProduct(int productId)
        {
            var product = await _dbContext.ShopProducts.FindAsync(productId);
            if (product == null || product.IsDisabled)
                return new ApiResult<bool> { ResponseCode = 404, ErrorMessage = "Product not found", Data = false };
            product.IsDisabled = true;
            _dbContext.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
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
