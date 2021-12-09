using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public ProductRepository(ApplicationDbContext context, Mapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<ProductDTO> GetProductAsync(Guid id)
        {
            return _mapper.MapToProductDTO(await FindProductByIdAsync(id));
        }

        public async Task<List<ProductDTO>> GetProductsAsync(string keyword)
        {
            return (await _dbContext.ShopProducts.AsNoTracking().Include(e => e.Category)
                .Where(product => product.ProductName.Contains(keyword)
                        || product.Category.CategoryName.Contains(keyword))
                .ToListAsync())
                .Select(product => _mapper.MapToProductDTO(product))
                .ToList();
        }
        public async Task<List<ProductDTO>> GetAllProductAsync()
        {
            return (await _dbContext.ShopProducts.AsNoTracking().Include(e => e.Category)
                .ToListAsync()).Select(product => _mapper.MapToProductDTO(product))
                .ToList();
        }

        public async Task<CommandResponse<bool>> AddProductAsync(AddOrEditProductRequestModel requestModel)
        {
            _dbContext.ShopProducts.Add(new ShopProduct().AssignByRequestModel(requestModel));
            try
            {
                await _dbContext.SaveChangesAsync();
                return new CommandResponse<bool>
                {
                    Response = true
                };
            }
            catch (Exception e)
            {
                return new CommandResponse<bool> { Response = false, Exception = e, ErrorMessage = e.Message };
            }
        }

        public async Task<CommandResponse<bool>> DeleteProductAsync(Guid id)
        {
            var product = await FindProductByIdAsync(id);
            if (product == null || product.IsDisabled)
                return new CommandResponse<bool> { Response = false, ErrorMessage = "Product is not found or already disabled" };
            product.IsDisabled = true;
            _dbContext.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return new CommandResponse<bool> { Response = true };
        }

        public async Task<CommandResponse<bool>> ActiveProductAsync(Guid id)
        {
            var product = await FindProductByIdAsync(id);
            if (product == null || !product.IsDisabled)
                return new CommandResponse<bool> { Response = false, ErrorMessage = "Product is not found or already enabled" };
            product.IsDisabled = false;
            _dbContext.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return new CommandResponse<bool> { Response = true };
        }

        public async Task<CommandResponse<bool>> EditProductAsync(Guid id, AddOrEditProductRequestModel requestModel)
        {
            var product = await FindProductByIdAsync(id);
            if (product == null || product.IsDisabled)
                return new CommandResponse<bool> { Response = false, ErrorMessage = "Product is not found or already disabled" };
            product.AssignByRequestModel(requestModel);
            try
            {
                await _dbContext.SaveChangesAsync();
                return new CommandResponse<bool> { Response = true };
            }
            catch (Exception e)
            {
                return new CommandResponse<bool> { Response = false, ErrorMessage = e.Message, Exception = e };
            }
        }

        private async Task<ShopProduct> FindProductByIdAsync(Guid id)
        {
            return await _dbContext.ShopProducts.FindAsync(id);
        }
    }
}
