using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Interfaces
{
    public interface IShopInterfaceRepository : IDisposable
    {
        Task<CommandResponse<int>> EditShopInterfaceAsync(int shopId,
            CreateOrEditShopInterfaceRequestModel requestModel);

        Task<ShopInterfaceDTO> FindShopInterfaceByShopId(int shopId);
    }
}
