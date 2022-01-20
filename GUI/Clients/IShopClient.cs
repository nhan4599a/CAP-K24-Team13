using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IShopClient
    {
        [Get("/shop/search?keyword={keyword}&paginationInfo.pageNumber={pageNumber}&paginationInfo.pageSize={pageSize}")]
        Task<ApiResponse<PaginatedList<ShopDTO>>> FindShops(string keyword, int pageNumber, int? pageSize);

        [Get("/interfaces/{shopId}")]
        Task<ApiResponse<ApiResult<ShopInterfaceDTO>>> FindInformation(int shopId);

        [Get("/shop/{shopId}")]
        Task<ApiResult<ShopDTO>> GetShop(int shopId);
    }
}
