using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
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

        [HttpGet("{id}")]
        public async Task<ApiResult<ProductDTO>> GetProduct(string id)
        {
            var result = await _dbContext.ShopProducts.FirstOrDefaultAsync(p => p.Id == id);
            if (result == null) return new ApiResult<ProductDTO> { ResponseCode = 404, Data = null };
            var category = await _dbContext.ShopCategories.FirstOrDefaultAsync(c => c.Id == result.CategoryId);

            var imageSet = result.ImageSet != null ? new string[] { result.ImageSet.Image1, result.ImageSet.Image2, result.ImageSet.Image3, result.ImageSet.Image3, result.ImageSet.Image4, result.ImageSet.Image5 } : null;
            var productDto = new ProductDTO()
            {
                CategoryName = category.CategoryName,
                ProductName = result.ProductName,
                Id = result.Id,
                Description = result.Description,
                Discount = result.Discount,
                Images = imageSet,
                Price = result.Price,
                Quantity = result.Quantity
            };
            return new ApiResult<ProductDTO>
            {
                ResponseCode = 200,
                Data = productDto
            };
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