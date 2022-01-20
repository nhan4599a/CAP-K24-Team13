using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IProductClient
    {
        [Get("/products/search?paginationInfo.pageNumber={pageNumber}&paginationInfo.pageSize={pageSize}")]
        Task<ApiResult<PaginatedList<ProductDTO>>> GetProductsAsync(int pageNumber, int? pageSize);

        [Get("/products/{productId}")]
        Task<ApiResponse<ApiResult<ProductDTO>>> GetProductAsync(string productId);

        [Get("/products/shop/{shopId}")]
        Task<ApiResponse<ApiResult<PaginatedList<ProductDTO>>>> GetProductsOfShopAsync(int shopId);

        [Get("/products/search?keyword={keyword}&paginationInfo.pageNumber={pageNumber}&paginationInfo.pageSize={pageSize}")]
        Task<ApiResponse<ApiResult<PaginatedList<ProductDTO>>>> FindProducts(string keyword, int pageNumber, int? pageSize);
    }
}