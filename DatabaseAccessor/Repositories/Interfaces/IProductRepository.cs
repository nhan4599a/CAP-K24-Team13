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
        public Task<ProductDTO> GetProductAsync(Guid id);

        public Task<List<ProductDTO>> GetProductsAsync(string keyword);

        public Task<List<ProductDTO>> GetAllProductAsync();

        public Task<CommandResponse<bool>> AddProductAsync(AddOrEditProductRequestModel requestModel);

        public Task<CommandResponse<bool>> DeleteProductAsync(Guid id);

        public Task<CommandResponse<bool>> ActiveProductAsync(Guid id);

        public Task<CommandResponse<bool>> EditProductAsync(Guid id, AddOrEditProductRequestModel requestModel);
    }
}
