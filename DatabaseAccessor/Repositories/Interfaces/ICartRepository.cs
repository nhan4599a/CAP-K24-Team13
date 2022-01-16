using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Interfaces
{
    public interface ICartRepository : IDisposable
    {
        Task<CommandResponse<bool>> AddProductToCartAsync(AddOrEditQuantityCartItemRequestModel requestModel);

        Task<CommandResponse<bool>> EditQuantityAsync(AddOrEditQuantityCartItemRequestModel requestModel);

        Task<CommandResponse<bool>> RemoveCartItemAsync(RemoveCartItemRequestModel requestModel);
        
        Task<CartDTO> GetCartAsync(string userId);
    }
}
