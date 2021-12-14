using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Interfaces
{
    public interface IProductRepository : IDisposable
    {
        Task<ProductDTO> GetProductAsync(Guid id);

        Task<List<ProductDTO>> GetProductsAsync(string keyword);

        Task<List<ProductDTO>> GetAllProductAsync();

        Task<CommandResponse<bool>> AddProductAsync(AddOrEditProductRequestModel requestModel);

        Task<CommandResponse<bool>> ActivateProductAsync(Guid id, bool isActivateCommand);

        Task<CommandResponse<bool>> EditProductAsync(Guid id, AddOrEditProductRequestModel requestModel);
    }
}
