using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductDTO> GetProductAsync(Guid id);

        Task<List<ProductDTO>> GetProductsAsync(string keyword);

        Task<List<ProductDTO>> GetAllProductAsync();

        Task<CommandResponse<bool>> AddProductAsync(AddOrEditProductRequestModel requestModel);

        Task<CommandResponse<bool>> ActiveProductAsync(Guid id, bool shouldDisable);

        Task<CommandResponse<bool>> EditProductAsync(Guid id, AddOrEditProductRequestModel requestModel);
    }
}
