using Refit;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IShopInterfaceClient
    {
        [Get("/interfaces/avatar")]
        Task<ApiResponse<ApiResult<Dictionary<int, string>>>> GetShopAvatar([Query(CollectionFormat.Multi)] int[] shopId);
    }
}
