using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
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

        public async Task<ProductWithCommentsDTO> GetProductAsync(Guid productId)
        {
            return _mapper.MapToProductWithCommentsDTO(await FindProductByIdAsync(productId));
        }

        public async Task<MinimalProductDTO> GetMinimalProductAsync(Guid productId)
        {
            return _mapper.MapToMinimalProductDTO(await FindProductByIdAsync(productId));
        }

        public async Task<PaginatedList<ProductDTO>> FindProductsAsync(string keyword, PaginationInfo paginationInfo)
        {
            return await _dbContext.ShopProducts.AsNoTracking().Include(e => e.Category)
                .Where(product => EF.Functions.Like(product.ProductName, $"%{keyword}%")
                        || EF.Functions.Like(product.Category.CategoryName, $"%{keyword}%"))
                .Select(product => _mapper.MapToProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public async Task<PaginatedList<ProductDTO>> GetAllProductAsync(PaginationInfo paginationInfo)
        {
            return await _dbContext.ShopProducts.AsNoTracking()
                .Include(product => product.Category)
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
            shopProduct.ShopId = category.ShopId;
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

        public async Task<CommandResponse<bool>> ActivateProductAsync(Guid productId, bool isActivateCommand)
        {
            var product = await FindProductByIdAsync(productId);
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

        public async Task<CommandResponse<ProductDTO>> EditProductAsync(Guid productId,
            CreateOrEditProductRequestModel requestModel)
        {
            var product = await FindProductByIdAsync(productId);
            if (product == null)
                return CommandResponse<ProductDTO>.Error("Product is not found", null);
            if (product.IsDisabled)
                return CommandResponse<ProductDTO>.Error("Product is disabled", null);
            if (product.Category.IsDisabled)
                return CommandResponse<ProductDTO>.Error($"Product is belong to {product.Category.CategoryName} which was deactivated", null);
            var category = await _dbContext.ShopCategories.FindAsync(requestModel.CategoryId);
            if (category == null)
                return CommandResponse<ProductDTO>.Error($"Category is not found", null);
            if (category.IsDisabled)
                return CommandResponse<ProductDTO>.Error($"Category is disabled", null);
            product.AssignByRequestModel(requestModel);
            product.ShopId = category.ShopId;
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

        public async Task<CommandResponse<int>> ImportProductQuantityAsync(Guid productId, int quantity)
        {
            var product = await FindProductByIdAsync(productId);
            if (product == null)
                return CommandResponse<int>.Error("Product is not found", null);
            if (product.IsDisabled)
                return CommandResponse<int>.Error("Product is disabled", null);
            if (product.Category.IsDisabled)
                return CommandResponse<int>.Error($"Product is belong to " +
                    $"{product.Category.CategoryName} which was deactivated", null);
            if (quantity <= 0)
                return CommandResponse<int>.Error("Quantity must greater than 0", null);
            var newQuantity = quantity + product.Quantity;
            product.Quantity = newQuantity;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<int>.Success(newQuantity);
        }

        public async Task<PaginatedList<ProductDTO>> GetAllProductsOfShopAsync(int shopId, PaginationInfo paginationInfo)
        {
            var result = await _dbContext.ShopProducts
                .AsNoTracking()
                .Include(product => product.Category)
                .Where(product => product.Category.ShopId == shopId)
                .Select(product => _mapper.MapToProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
            return result;
        }

        public async Task<PaginatedList<ProductDTO>> FindProductsOfShopAsync(int shopId, string keyword, PaginationInfo paginationInfo)
        {
            var result = await _dbContext.ShopProducts
                .AsNoTracking()
                .Include(product => product.Category)
                .Where(product => product.ShopId == shopId &&
                    (EF.Functions.Like(product.ProductName, $"%{product.ProductName}%")
                        || EF.Functions.Like(product.Category.CategoryName, $"%{product.Category.CategoryName}%")))
                .Select(product => _mapper.MapToProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
            return result;
        }

        private async Task<ShopProduct> FindProductByIdAsync(Guid id)
        {
            return await _dbContext.ShopProducts.FindAsync(id);
        }

        public async Task<CommandResponse<List<ProductDTO>>> GetRelatedProductsAsync(Guid productId)
        {
            var sourceProduct = await _dbContext.ShopProducts.FindAsync(productId);
            if (sourceProduct == null)
                return CommandResponse<List<ProductDTO>>.Error("Product is not found!", null);
            var result = await _dbContext.ShopProducts.Where(product => product.ShopId == sourceProduct.ShopId
                    && product.CategoryId == sourceProduct.CategoryId && product.Id != sourceProduct.Id)
                .OrderBy(product => product.Invoices.Count(invoice => invoice.Invoice.Status == InvoiceStatus.Succeed))
                .Take(4).Select(product => _mapper.MapToProductDTO(product))
                .ToListAsync();
            return CommandResponse<List<ProductDTO>>.Success(result);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
