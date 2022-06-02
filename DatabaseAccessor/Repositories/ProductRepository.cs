using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using Shared.Extensions;
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
            return _mapper.MapToProductWithCommentsDTO(await FindProductByIdAsync(productId, true));
        }

        public async Task<MinimalProductDTO> GetMinimalProductAsync(Guid productId)
        {
            return _mapper.MapToMinimalProductDTO(await FindProductByIdAsync(productId, false));
        }

        public async Task<PaginatedList<ProductDTO>> FindProductsAsync(string keyword, PaginationInfo paginationInfo)
        {
            return await _dbContext.ShopProducts.AsNoTracking()
                .Include(e => e.Comments)
                .AsSplitQuery()
                .Where(product => EF.Functions.Like(product.ProductName, $"%{keyword}%")
                        || EF.Functions.Like(product.Category, $"%{keyword}%"))
                .OrderByDescending(product => product.CreatedDate)
                .Select(product => _mapper.MapToProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public async Task<PaginatedList<ProductDTO>> GetAllProductAsync(PaginationInfo paginationInfo)
        {
            return await _dbContext.ShopProducts.AsNoTracking()
                .Include(e => e.Comments)
                .AsSplitQuery()
                .OrderByDescending(product => product.CreatedDate)
                .Select(product => _mapper.MapToProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public async Task<CommandResponse<Guid>> AddProductAsync(CreateProductRequestModel requestModel)
        {
            var shopStatus = await _dbContext.ShopStatus.AsNoTracking()
                .FirstOrDefaultAsync(shop => shop.ShopId == requestModel.ShopId && !shop.IsDisabled);
            if (shopStatus == null)
                return CommandResponse<Guid>.Error("Shop is already disabled!", null);
            var shopProduct = new ShopProduct().AssignByRequestModel(requestModel);
            if (await _dbContext.ShopProducts.AnyAsync(product => product.ShopId == shopProduct.ShopId && 
                product.Category == shopProduct.Category && product.ProductName == shopProduct.ProductName))
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

        public async Task<CommandResponse<bool>> ActivateProductAsync(Guid productId, bool isActivateCommand)
        {
            var product = await FindProductByIdAsync(productId, false);
            if (product == null)
                return CommandResponse<bool>.Error("Product is not found", null);
            if (isActivateCommand && !product.IsDisabled)
                return CommandResponse<bool>.Error("Product is already activated", null);
            if (!isActivateCommand && product.IsDisabled)
                return CommandResponse<bool>.Error("Product is already deactivated", null);
            if (!product.IsVisible)
                return CommandResponse<bool>.Error("Shop is already disabled!", null);
            product.IsDisabled = !isActivateCommand;
            _dbContext.Entry(product).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(isActivateCommand);
        }

        public async Task<CommandResponse<ProductDTO>> EditProductAsync(Guid productId,
            EditProductRequestModel requestModel)
        {
            var product = await FindProductByIdAsync(productId, false);
            if (product == null)
                return CommandResponse<ProductDTO>.Error("Product is not found", null);
            if (product.IsDisabled)
                return CommandResponse<ProductDTO>.Error("Product is disabled", null);
            if (!product.IsVisible)
                return CommandResponse<ProductDTO>.Error("Shop is already disabled!", null);
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

        public async Task<CommandResponse<int>> ImportProductQuantityAsync(Guid productId, int quantity)
        {
            var product = await FindProductByIdAsync(productId, false);
            if (product == null)
                return CommandResponse<int>.Error("Product is not found", null);
            if (product.IsDisabled)
                return CommandResponse<int>.Error("Product is disabled", null);
            if (!product.IsVisible)
                return CommandResponse<int>.Error("Shop is already disabled!", null);
            if (quantity <= 0)
                return CommandResponse<int>.Error("Quantity must greater than 0", null);
            var newQuantity = quantity + product.Quantity;
            product.Quantity = newQuantity;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<int>.Success(newQuantity);
        }

        public async Task<PaginatedList<ProductDTO>> GetAllProductsOfShopAsync(int shopId, PaginationInfo paginationInfo, bool includeFilter)
        {
            IQueryable<ShopProduct> source = _dbContext.ShopProducts;
            if (!includeFilter)
                source = source.IgnoreQueryFilters();
            var result = await source
                .AsNoTracking()
                .Include(e => e.Comments)
                .AsSplitQuery()
                .Where(product => product.ShopId == shopId)
                .OrderByDescending(product => product.CreatedDate)
                .Select(product => _mapper.MapToProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
            return result;
        }

        public async Task<PaginatedList<ProductDTO>> FindProductsOfShopAsync(int shopId, string keyword, PaginationInfo paginationInfo, bool includeFilter)
        {
            IQueryable<ShopProduct> source = _dbContext.ShopProducts;
            if (!includeFilter)
                source = source.IgnoreQueryFilters();
            var result = await source
                .AsNoTracking()
                .Include(e => e.Comments)
                .AsSplitQuery()
                .Where(product => product.ShopId == shopId)
                .Where(product => EF.Functions.Like(product.ProductName, $"%{keyword}%")
                        || EF.Functions.Like(product.Category, $"%{keyword}%"))
                .OrderByDescending(product => product.CreatedDate)
                .Select(product => _mapper.MapToProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
            return result;
        }

        private async Task<ShopProduct> FindProductByIdAsync(Guid id, bool includeComments)
        {
            var source = _dbContext.ShopProducts.IgnoreQueryFilters();
            if (includeComments)
                source.Include(e => e.Comments);
            return await source.Where(product => product.Id == id).FirstOrDefaultAsync();
        }

        public async Task<CommandResponse<List<ProductDTO>>> GetRelatedProductsAsync(Guid productId)
        {
            var sourceProduct = await _dbContext.ShopProducts.FindAsync(productId);
            if (sourceProduct == null)
                return CommandResponse<List<ProductDTO>>.Error("Product is not found!", null);
            var relatedCategoriesId = GetRelatedCategories(sourceProduct.CategoryId);
            var result = await _dbContext.ShopProducts
                .AsNoTracking()
                .Include(e => e.Comments)
                .AsSplitQuery()
                .Where(product => product.Id != sourceProduct.Id)
                .Where(product => relatedCategoriesId.Contains(product.CategoryId))
                .OrderByDescending(product => product.Invoices.Count(invoice => invoice.Invoice.Status == InvoiceStatus.Succeed))
                .ThenByDescending(product => product.CreatedDate)
                .Take(4).Select(product => _mapper.MapToProductDTO(product))
                .ToListAsync();
            return CommandResponse<List<ProductDTO>>.Success(result);
        }

        public async Task<List<MinimalProductDTO>> GetBestSellerProductsAsync(int? shopId)
        {
            return await _dbContext.ShopProducts
                .AsNoTracking()
                .Where(product => !shopId.HasValue || product.ShopId == shopId.Value)
                .OrderByDescending(product => product.Invoices.Count(invoice => invoice.Invoice.Status == InvoiceStatus.Succeed))
                .ThenByDescending(product => product.CreatedDate)
                .Take(5)
                .Select(product => _mapper.MapToMinimalProductDTO(product))
                .ToListAsync();
        }

        public async Task<PaginatedList<ProductDTO>> GetProductsOfCategoryAsync(int? shopId, List<int> categoryIds, string keyword, string orderByFieldName, OrderByDirection orderByDirection, PaginationInfo paginationInfo)
        {
            IQueryable<ShopProduct> source = _dbContext.ShopProducts.AsNoTracking()
                .Include(e => e.Comments)
                .AsSplitQuery();
            if (shopId.HasValue)
                source = source.Where(product => product.ShopId == shopId);
            if (!string.IsNullOrWhiteSpace(keyword))
                source = source.Where(product => EF.Functions.Like(product.ProductName, $"%{keyword}%"));
            source = source
                .Where(product => !categoryIds.Any() || categoryIds.Contains(product.CategoryId));
            if (orderByDirection != OrderByDirection.Unspecified)
                source = source
                    .OrderBy(orderByFieldName, orderByDirection)
                    .ThenByDescending(product => product.CreatedDate);
            else
                source = source.OrderByDescending(product => product.CreatedDate);
            return await source
                .Select(product => _mapper.MapToProductDTO(product))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public async Task<List<MinimalProductDTO>> GetTopMostSaleOffProductsAsync()
        {
            return await _dbContext.ShopProducts
                .AsNoTracking()
                .OrderByDescending(product => product.Discount)
                .ThenBy(product => product.Invoices.Count(invoice => invoice.Invoice.Status == InvoiceStatus.Succeed))
                .Take(2)
                .Select(product => _mapper.MapToMinimalProductDTO(product))
                .ToListAsync();
        }

        public async Task<List<ProductDTO>> GetTopNewsProductsAsync()
        {
            return await _dbContext.ShopProducts
                .AsNoTracking()
                .Include(e => e.Comments)
                .Where(product => product.CreatedDate >= DateTime.Now.AddHours(7).AddDays(-3))
                .OrderByDescending(product => product.CreatedDate)
                .Take(2)
                .Select(product => _mapper.MapToProductDTO(product))
                .ToListAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        private static List<int> GetRelatedCategories(int categoryId)
        {
            var superCategories = new int[][]
            {
                new int[]
                {
                    1, 2, 5, 7, 8, 9, 11, 12, 13, 17, 18
                },
                new int[]
                {
                    6, 9, 10, 14, 16
                },
                new int[]
                {
                    4, 3
                },
                new int[]
                {
                    3, 4, 1 ,8, 11, 2, 7, 12, 5, 13
                },
                new int[]
                {
                    19, 29, 14, 9, 28, 6
                },
                new int[]
                {
                    28, 19, 14
                },
                new int[]
                {
                    26, 25
                },
                new int[]
                {
                    21, 22, 24, 3, 4, 6, 15, 14, 20, 23, 27, 29
                }
            };
            return superCategories
                .Where(superCategory => superCategory.Contains(categoryId))
                .SelectMany(e => e).Distinct().ToList();
        }
    }
}
