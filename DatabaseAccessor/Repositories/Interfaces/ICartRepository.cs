using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Interfaces
{
    public interface ICartRepository : IDisposable
    {
        Task<CommandResponse<bool>> AddProductToCart(AddOrEditQuantityCartItemRequestModel requestModel);

        Task<CommandResponse<bool>> EditQuantity(AddOrEditQuantityCartItemRequestModel requestModel);

        Task<CommandResponse<bool>> RemoveCartItem(RemoveCartItemRequestModel requestModel);
        
        Task<CartDTO> GetCartAsync(string userId);
    }
}
