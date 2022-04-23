using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IProductRepository : IDisposable
    {
        Task<ProductWithCommentsDTO> GetProductAsync(Guid productId);

        Task<MinimalProductDTO> GetMinimalProductAsync(Guid productId);

        Task<PaginatedList<ProductDTO>> FindProductsAsync(string keyword, PaginationInfo paginationInfo);

        Task<PaginatedList<ProductDTO>> GetAllProductAsync(PaginationInfo paginationInfo);

        Task<CommandResponse<Guid>> AddProductAsync(CreateProductRequestModel requestModel);

        Task<CommandResponse<bool>> ActivateProductAsync(Guid productId, bool isActivateCommand);

        Task<CommandResponse<ProductDTO>> EditProductAsync(Guid productId, EditProductRequestModel requestModel);

        Task<PaginatedList<ProductDTO>> GetAllProductsOfShopAsync(int shopId, PaginationInfo pagination, bool includeFilter);

        Task<PaginatedList<ProductDTO>> FindProductsOfShopAsync(int shopId, string keyword, PaginationInfo paginationInfo, bool includeFilter);

        Task<CommandResponse<List<ProductDTO>>> GetRelatedProductsAsync(Guid productId);

        Task<List<MinimalProductDTO>> GetBestSellerProductsAsync(int? shopId);

        Task<CommandResponse<int>> ImportProductQuantityAsync(Guid productId, int quantity);

        Task<PaginatedList<ProductDTO>> GetProductsOfCategoryAsync(int? shopId, List<int> categoryIds, string keyword, string orderByFieldName, OrderByDirection orderByDirection, PaginationInfo paginationInfo);

        Task<List<MinimalProductDTO>> GetTopMostSaleOffProductsAsync();

        Task<List<ProductDTO>> GetTopNewsProductsAsync();
    }
}
