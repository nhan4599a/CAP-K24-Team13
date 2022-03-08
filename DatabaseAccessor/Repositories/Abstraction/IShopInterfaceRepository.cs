using DatabaseAccessor.Contexts;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IShopInterfaceRepository : IEFCoreRepository<ApplicationDbContext>
    {
        Task<CommandResponse<ShopInterfaceDTO>> EditShopInterfaceAsync(int shopId,
            CreateOrEditInterfaceRequestModel requestModel);

        Task<CommandResponse<ShopInterfaceDTO>> FindShopInterfaceByShopIdAsync(int shopId);
    }
}
