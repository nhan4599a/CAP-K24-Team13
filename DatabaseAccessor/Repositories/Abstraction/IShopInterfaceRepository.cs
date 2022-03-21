using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IShopInterfaceRepository : IDisposable
    {
        Task<CommandResponse<ShopInterfaceDTO>> EditShopInterfaceAsync(int shopId,
            CreateOrEditInterfaceRequestModel requestModel);

        Task<CommandResponse<ShopInterfaceDTO>> FindShopInterfaceByShopIdAsync(int shopId);
    }
}
