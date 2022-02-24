using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IOrderClient
    {
        [Get("/orders/user/{userId}")]
        Task<ApiResponse<ApiResult<List<OrderItemDTO>>>> GetOrderUserHistory([Header("Authorization: Bearer")] string token, string userId);

        [Get("/orders/shop/{shopId}")]
        Task<ApiResponse<ApiResult<List<OrderDTO>>>> GetNearByOrders([Header("Authorization: Bearer")] string token, int shopId);
    }
}
