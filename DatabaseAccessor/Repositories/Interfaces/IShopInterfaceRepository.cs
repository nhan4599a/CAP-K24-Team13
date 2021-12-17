using Shared;
using Shared.RequestModels;
using System;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Interfaces
{
    public interface IShopInterfaceRepository : IDisposable
    {
        Task<CommandResponse<bool>> AddShopInterfaceAsync(CreateOrEditShopInterfaceRequestModel requestModel);

        Task<CommandResponse<bool>> EditShopInterfaceAsync(int interfaceId, CreateOrEditShopInterfaceRequestModel requestModel);
    }
}
