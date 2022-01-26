using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface ICartRepository : IDisposable
    {
        Task<CommandResponse<bool>> AddProductToCartAsync(AddOrEditQuantityCartItemRequestModel requestModel);

        Task<CommandResponse<bool>> EditQuantityAsync(AddOrEditQuantityCartItemRequestModel requestModel);

        Task<CommandResponse<bool>> RemoveCartItemAsync(RemoveCartItemRequestModel requestModel);
        
        Task<List<CartItemDTO>> GetCartAsync(string userId);

        Task<int> GetCartItemCountAsync(string userId);
    }
}
