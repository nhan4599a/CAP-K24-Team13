using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI
{
    public interface IProductClient
    {
        [Get("/products/search?requestModel.paginationInfo.pageNumber={pageNumber}&requestModel.paginationInfo.pageSize={pageSize}")]
        Task<ApiResult<PaginatedList<ProductDTO>>> GetProductsAsync(int pageNumber, int? pageSize);

        [Get("/products/{productId}")]
        Task<ApiResult<ProductDTO>> GetProductAsync(string productId);
    }
} 