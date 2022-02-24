using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface ICartClient
    {
        [Get("/cart/{userId}/items")]
        Task<ApiResponse<ApiResult<List<CartItemDTO>>>> GetCartItemsAsync([Header("Authorization: Bearer")] string token, string userId);
    }
}
