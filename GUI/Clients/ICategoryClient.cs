using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface ICategoryClient
    {
        [Get("/categories/shop/{shopId}?pageSize={pageSize}")]
        Task<ApiResponse<ApiResult<PaginatedList<CategoryDTO>>>> GetCategoriesOfShop(int shopId, int pageSize);

        [Get("/categories?pageSize={pageSize}")]
        Task<ApiResponse<ApiResult<PaginatedList<CategoryDTO>>>> GetCategories(int pageSize);
    }
}
