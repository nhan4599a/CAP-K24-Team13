using Refit;
using Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface ICartClient
    {
        [Get("/cart/{userId}/count")]
        Task<ApiResponse<int>> GetCartItemCountAsync(string userId);

        [Get("/cart/{userId}/items")]
        Task<ApiResponse<List<CartItemDTO>>> GetCartItemsAsync(string userId);
    }
}
