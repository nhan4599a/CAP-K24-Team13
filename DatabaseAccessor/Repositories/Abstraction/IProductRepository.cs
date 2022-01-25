using Shared;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IProductRepository : IDisposable
    {
        Task<ProductDTO> GetProductAsync(Guid id);

        Task<PaginatedList<ProductDTO>> GetProductsAsync(string keyword, PaginationInfo paginationInfo);

        Task<PaginatedList<ProductDTO>> GetAllProductAsync(PaginationInfo paginationInfo);

        Task<CommandResponse<Guid>> AddProductAsync(CreateOrEditProductRequestModel requestModel);

        Task<CommandResponse<bool>> ActivateProductAsync(Guid id, bool isActivateCommand);

        Task<CommandResponse<ProductDTO>> EditProductAsync(Guid id, CreateOrEditProductRequestModel requestModel);

        Task<PaginatedList<ProductDTO>> GetAllProductsOfShopAsync(int shopId, PaginationInfo pagination);
    }
}
