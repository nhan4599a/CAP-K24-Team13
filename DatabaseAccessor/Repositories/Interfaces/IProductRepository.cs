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

        Task<CommandResponse<Guid>> AddProductAsync(CreateOrEditProductRequestModel requestModel);

        Task<CommandResponse<bool>> ActivateProductAsync(Guid id, bool isActivateCommand);

        Task<CommandResponse<ProductDTO>> EditProductAsync(Guid id, CreateOrEditProductRequestModel requestModel);
    }
}
