using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI
{
    public interface IProductClient
    {
        [Get("/products?requestModel.paginationInfo.pageNumber={pageNumber}&requestModel.paginationInfo.pageSize={pageSize}")]
        Task<ApiResult<PaginatedList<ProductDTO>>> GetProductsAsync(int pageNumber, int? pageSize);
    }
} 