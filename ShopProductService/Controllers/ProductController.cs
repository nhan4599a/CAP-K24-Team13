using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using ShopProductService.RequestModel;
using System.Collections.Generic;
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
        public async Task<ApiResult<bool>> AddProduct(AddProductRequestModel requestModel)
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
            await _dbContext.SaveChangesAsync();
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }
        
        [HttpPut]
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
        public async Task<ApiResult<bool>> DeleteProduct([FromBody] string productId)
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
        public async Task<ApiResult<PaginatedDataList<ProductDTO>>> ListProduct([FromQuery] SearchProductRequestModel requestModel)
        {
            var keyword = requestModel.Keyword;
            var productList = new List<ProductDTO>();
            if (!string.IsNullOrEmpty(keyword))
            {
                productList = await _dbContext.ShopProducts.AsNoTracking()
                                .Where(product => product.ProductName.Contains(keyword) ||
                                        product.Category.CategoryName.Contains(keyword))
                                .Select(product => ProductDTO.FromSource(product))
                                .Cast<ProductDTO>()
                                .ToListAsync();
            }
            else
            {
                productList = await _dbContext.ShopProducts.AsNoTracking()
                                .Select(product => ProductDTO.FromSource(product))
                                .Cast<ProductDTO>()
                                .ToListAsync();
            }
            return new ApiResult<PaginatedDataList<ProductDTO>>
            {
                ResponseCode = 200,
                Data = productList.Paginate(requestModel.PaginationInfo.PageNumber, requestModel.PaginationInfo.PageSize)
            };
        }
    }
}