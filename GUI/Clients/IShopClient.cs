using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IShopClient
    {
        [Get("/shop/search?keyword={keyword}&pageNumber={pageNumber}&pageSize={pageSize}")]
        Task<ApiResponse<ApiResult<PaginatedList<ShopDTO>>>> FindShops(string keyword, int pageNumber, int? pageSize);

        [Get("/shops/{shopId}")]
        Task<ApiResponse<ApiResult<ShopDTO>>> GetShop(int shopId);
    }
}
