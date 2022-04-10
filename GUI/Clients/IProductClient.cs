using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IProductClient
    {
        [Get("/products/search?pageNumber={pageNumber}&pageSize={pageSize}")]
        Task<ApiResponse<ApiResult<PaginatedList<ProductDTO>>>> GetProductsAsync(int pageNumber, int? pageSize);

        [Get("/products/{productId}")]
        Task<ApiResponse<ApiResult<ProductWithCommentsDTO>>> GetProductAsync(string productId);

        [Get("/products/less/{productId}")]
        Task<ApiResponse<ApiResult<ProductDTO>>> GetProductInfoInCheckout([Authorize] string token, string productId);

        [Get("/products/shop/{shopId}/search?pageSize=0")]
        Task<ApiResponse<ApiResult<PaginatedList<ProductDTO>>>> GetProductsOfShopAsync(int shopId);

        [Get("/products/search?keyword={keyword}&pageNumber={pageNumber}&pageSize={pageSize}")]
        Task<ApiResponse<ApiResult<PaginatedList<ProductDTO>>>> FindProducts(string keyword, int pageNumber, int? pageSize);

        [Get("/products/best")]
        Task<ApiResponse<ApiResult<List<MinimalProductDTO>>>> GetBestSellerProducts([Query] int? shopId);

        [Get("/products/category?pageNumber={pageNumber}&pageSize={pageSize}")]
        Task<ApiResponse<ApiResult<PaginatedList<ProductDTO>>>> GetProductsOfCategory([Query(CollectionFormat.Multi)] int[] categoryId, int pageNumber, int pageSize);
    }
}