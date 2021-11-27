using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.DTOs;
using System.Linq;
using System.Collections.Generic;
using ShopProductService.RequestModel;

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
        public ApiResult<bool> AddProduct(AddProductRequestModel requestModel)
        {
            _dbContext.ShopProducts.Add(new ShopProduct
            {
                ProductName = requestModel.ProductName,
                CategoryId = requestModel.CategoryId,
                Description = requestModel.Description,
                Quantity = requestModel.Quantity,
                Price = requestModel.Price,
                Discount = requestModel.Discount
            });
            _dbContext.SaveChangesAsync();
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
        public async Task<ApiResult<bool>> DeleteProduct([FromQuery] int productId)
        {
            var product = await _dbContext.ShopProducts.FindAsync(productId);
            if (product == null || product.IsDisabled)
                return new ApiResult<bool> { ResponseCode = 404, ErrorMessage = "Product not found", Data = false };
            product.IsDisabled = true;
            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
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