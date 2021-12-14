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
using Microsoft.EntityFrameworkCore.Infrastructure;
using EntityFrameworkCore.Triggered;

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
            return await _dbContext.ShopProducts.AsNoTracking().Include(e => e.Category)
                .Where(product => product.ProductName.Contains(keyword)
                        || product.Category.CategoryName.Contains(keyword))
                .Select(product => _mapper.MapToProductDTO(product))
                .ToListAsync();
        }

        public async Task<List<ProductDTO>> GetAllProductAsync()
        {
            return await _dbContext.ShopProducts.AsNoTracking()
                .Include(e => e.Category)
                .Select(product => _mapper.MapToProductDTO(product))
                .ToListAsync();
        }

        public async Task<CommandResponse<bool>> AddProductAsync(AddOrEditProductRequestModel requestModel)
        {
            var category = await _dbContext.ShopCategories.FindAsync(requestModel.CategoryId);
            if (category == null)
                return new CommandResponse<bool> { Response = false, ErrorMessage = "Category is not found" };
            if (category.IsDisabled)
                return new CommandResponse<bool> { Response = false, ErrorMessage = "Category is disabled" };
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

        public async Task<CommandResponse<bool>> ActivateProductAsync(Guid id, bool isActivateCommand)
        {
            var product = await FindProductByIdAsync(id);
            if (product == null)
                return new CommandResponse<bool> { Response = false, ErrorMessage = "Product is not found" };
            if (isActivateCommand && !product.IsDisabled)
                return new CommandResponse<bool> { Response = false, ErrorMessage = "Product is already activated" };
            if (!isActivateCommand && product.IsDisabled)
                return new CommandResponse<bool> { Response = false, ErrorMessage = "Product is already deactivated" };
            if (isActivateCommand && product.Category.IsDisabled)
                return new CommandResponse<bool> { Response = false, ErrorMessage = "Product is belong to a deactivated category" };
            product.IsDisabled = !isActivateCommand;
            _dbContext.Entry(product).State = EntityState.Modified;
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

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
