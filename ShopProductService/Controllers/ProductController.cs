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
using AutoMapper;
using Shared.Mapping;

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
        [ActionName("Edit")]
        public async Task<ApiResult<bool>> EditProduct(ProductDTO productDTO)
        {
            var product = await _dbContext.ShopProducts.FirstOrDefaultAsync(p => p.Id == productDTO.Id);
            if (product == null || product.IsDisabled)
                return new ApiResult<bool> { ResponseCode = 404, ErrorMessage = "Product not found", Data = false };
            var category = await _dbContext.ShopCategories.FirstOrDefaultAsync(c => c.CategoryName == productDTO.CategoryName);
            product.CategoryId = category != null ? category.Id : 0;
            product.ProductName = productDTO.ProductName;
            product.Description = productDTO.Description;
            product.Quantity = productDTO.Quantity;
            product.Price = productDTO.Price;
            product.Discount = productDTO.Discount;
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