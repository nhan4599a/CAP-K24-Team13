using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI
{
    public interface ICategoryClient
    {
        [Get("/categories/shop/{shopId}")]
        Task<ApiResponse<ApiResult<PaginatedList<CategoryDTO>>>> GetCategoriesOfShop(int shopId);
    }
}
