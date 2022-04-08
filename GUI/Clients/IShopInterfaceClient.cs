using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IShopInterfaceClient
    {
        [Get("/interfaces")]
        Task<ApiResponse<ApiResult<Dictionary<int, ShopInterfaceDTO>>>> GetShopInterface([Query(CollectionFormat.Multi)] int[] shopId);
    }
}
