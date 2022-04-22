using GUI.Models;
using Refit;
using Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IShopClient
    {
        Task<ApiResponse<List<ShopDTO>>> GetAllShops();

        Task<ApiResponse<ExternalApiPaginatedList<ShopDTO>>> FindShops(string keyword, int pageNumber, int pageSize);

        Task<ApiResponse<List<ShopDTO>>> FindShops(string keyword);

        Task<ApiResponse<ExternalApiResult<ShopDTO>>> GetShop(int shopId);
    }
}
