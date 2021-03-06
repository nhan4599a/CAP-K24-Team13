using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System;
using System.Linq;
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
            _mapper = mapper ?? Mapper.GetInstance();
        }

        public async Task<ProductWithCommentsDTO> GetProductAsync(Guid id)
        {
            return _mapper.MapToProductWithCommentsDTO(await FindProductByIdAsync(id));
        }

        public async Task<MinimalProductDTO> GetMinimalProductAsync(Guid id)
        {
            return _mapper.MapToMinimalProductDTO(await FindProductByIdAsync(id));
        }

        public async Task<PaginatedList<ProductDTO>> GetProductsAsync(string keyword, PaginationInfo paginationInfo)
        {
            return await _dbContext.ShopProducts.AsNoTracking().Include(e => e.Category)
                .Where(product => product.ProductName.Contains(keyword)
                        || product.Category.CategoryName.Contains(keyword))
                .Select(product => _mapper.MapToProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public async Task<PaginatedList<ProductDTO>> GetAllProductAsync(PaginationInfo paginationInfo)
        {
            return await _dbContext.ShopProducts.AsNoTracking()
                .Include(e => e.Category)
                .Select(product => _mapper.MapToProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public async Task<PaginatedList<MinimalProductDTO>> GetAllProductMinimalAsync(PaginationInfo paginationInfo)
        {
            return await _dbContext.ShopProducts.AsNoTracking()
                .Select(product => _mapper.MapToMinimalProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public async Task<CommandResponse<Guid>> AddProductAsync(CreateOrEditProductRequestModel requestModel)
        {
            var category = await _dbContext.ShopCategories.FindAsync(requestModel.CategoryId);
            if (category == null)
                return CommandResponse<Guid>.Error("Category is not found", null);
            if (category.IsDisabled)
                return CommandResponse<Guid>.Error("Category is disabled", null);
            var shopProduct = new ShopProduct().AssignByRequestModel(requestModel);
            if (await _dbContext.ShopProducts.AnyAsync(product => product.ShopId == shopProduct.ShopId && 
                product.CategoryId == shopProduct.CategoryId && product.ProductName == shopProduct.ProductName))
            {
                return CommandResponse<Guid>.Error("Product's name is already existed", null);
            }
            _dbContext.ShopProducts.Add(shopProduct);
            try
            {
                await _dbContext.SaveChangesAsync();
                return CommandResponse<Guid>.Success(shopProduct.Id);
            }
            catch (Exception e)
            {
                return CommandResponse<Guid>.Error(e.Message, e);
            }
        }

        public async Task<CommandResponse<bool>> ActivateProductAsync(Guid id, bool isActivateCommand)
        {
            var product = await FindProductByIdAsync(id);
            if (product == null)
                return CommandResponse<bool>.Error("Product is not found", null);
            if (isActivateCommand && !product.IsDisabled)
                return CommandResponse<bool>.Error("Product is already activated", null);
            if (!isActivateCommand && product.IsDisabled)
                return CommandResponse<bool>.Error("Product is already deactivated", null);
            if (isActivateCommand && product.Category.IsDisabled)
                return CommandResponse<bool>.Error($"Product is belong to {product.Category.CategoryName} which was deactivated", null);
            product.IsDisabled = !isActivateCommand;
            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(isActivateCommand);
        }

        public async Task<CommandResponse<ProductDTO>> EditProductAsync(Guid id, CreateOrEditProductRequestModel requestModel)
        {
            var product = await FindProductByIdAsync(id);
            if (product == null)
                return CommandResponse<ProductDTO>.Error("Product is not found", null);
            if (product.IsDisabled)
                return CommandResponse<ProductDTO>.Error("Product is disabled", null);
            if (product.Category.IsDisabled)
                return CommandResponse<ProductDTO>.Error($"Product is belong to {product.Category.CategoryName} which was deactivated", null);
            product.AssignByRequestModel(requestModel);
            try
            {
                await _dbContext.SaveChangesAsync();
                return CommandResponse<ProductDTO>.Success(_mapper.MapToProductDTO(product)); 
            }
            catch (Exception e)
            {
                return CommandResponse<ProductDTO>.Error(e.Message, e);
            }
        }

        private async Task<ShopProduct> FindProductByIdAsync(Guid id)
        {
            return await _dbContext.ShopProducts.FindAsync(id);
        }

        public async Task<PaginatedList<ProductDTO>> GetAllProductsOfShopAsync(int shopId, PaginationInfo paginationInfo)
        {
            var result = await _dbContext.ShopProducts
                .AsNoTracking()
                .Include(e => e.Category)
                .Where(product => product.Category.ShopId == shopId)
                .Select(product => _mapper.MapToProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
            return result;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
