using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.Mapping;
using ShopProductService.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/products")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public ProductController(ApplicationDbContext dbcontext, Mapper mapper)
        {
            _dbContext = dbcontext;
            _mapper = mapper;
        }
        
        [HttpPost]
        public async Task<ApiResult<bool>> AddProduct(AddOrEditProductRequestModel requestModel)
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
        
        [HttpPut("{id}")]
        public async Task<ApiResult<bool>> EditProduct(string id, AddOrEditProductRequestModel requestModel)
        {
            var product = await _dbContext.ShopProducts.FindAsync(id);
            if (product == null || product.IsDisabled)
                return new ApiResult<bool> { ResponseCode = 404, ErrorMessage = "Product not found", Data = false };
            product.CategoryId = requestModel.CategoryId;
            product.ProductName = requestModel.ProductName;
            product.Description = requestModel.Description;
            product.Quantity = requestModel.Quantity;
            product.Price = requestModel.Price;
            product.Discount = requestModel.Discount;
            bool result = await _dbContext.SaveChangesAsync() > 0;
            if (!result)
            {
                return new ApiResult<bool> { ResponseCode = 500, Data = false };
            }
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }
        
        [HttpDelete("{id}")]
        public async Task<ApiResult<bool>> DeleteProduct(string id)
        {
            var product = await _dbContext.ShopProducts.FindAsync(new Guid(id));
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
                productList = (await _dbContext.ShopProducts.AsNoTracking()
                                .Include(e => e.Category)
                                .Where(product => product.ProductName.Contains(keyword) || product.Category.CategoryName.Contains(keyword))
                                .ToListAsync())
                                .Select(product => _mapper.MapToProductDTO(product))
                                .ToList();
            }
            else
            {
                productList = (await _dbContext.ShopProducts.AsNoTracking()
                                .Include(e => e.Category)
                                .ToListAsync())
                                .Select(product => _mapper.MapToProductDTO(product))
                                .ToList();
            }
            return new ApiResult<PaginatedDataList<ProductDTO>>
            {
                ResponseCode = 200,
                Data = productList.Paginate(requestModel.PaginationInfo.PageNumber, requestModel.PaginationInfo.PageSize)
            };
        }

        [HttpGet("{id}")]
        public async Task<ApiResult<ProductDTO>> GetSingleProduct(string id)
        {
            var product = await _dbContext.ShopProducts.FindAsync(new Guid(id));
            if (product == null)
                return new ApiResult<ProductDTO> { ResponseCode = 404, ErrorMessage = "Product not found" };
            return new ApiResult<ProductDTO> { ResponseCode = 200, Data = _mapper.MapToProductDTO(product) };
        }
    }
}