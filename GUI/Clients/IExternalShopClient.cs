using GUI.Models;
using Refit;
using Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IExternalShopClient
    {
        [Get("/shops/getalllist")]
        Task<ApiResponse<List<ShopDTO>>> GetAllShops();

        [Get("/shops/paging?keyword={keyword}&pageIndex={pageNumber}&pageSize={pageSize}")]
        Task<ApiResponse<ExternalApiPaginatedList<ShopDTO>>> FindShops(string keyword, int pageNumber, int pageSize);

        [Get("/shops/publish/getalllist/{keyword}")]
        Task<ApiResponse<List<ShopDTO>>> FindShops(string keyword);

        [Get("/shops/{shopId}")]
        Task<ApiResponse<ExternalApiResult<ShopDTO>>> GetShop(int shopId);
    }
}
