using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI
{
    public interface IShopClient
    {
        [Get("/api/shop/search?keyword={keyword}&pageNumber={pageNumber}&pageSize={pageSize}")]
        Task<ApiResponse<PaginatedList<ShopDTO>>> FindShops(string keyword, int pageNumber, int? pageSize);
    }
}
