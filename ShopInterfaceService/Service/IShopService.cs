using Shared.DTOs;
using ShopInterfaceService.RequestModel;
using System.Threading.Tasks;

namespace ShopInterfaceService.Service
{
    public interface IShopService
    {
        public Task<ShopInterfaceDTO> EditShopInterfaceAsync(int shopId, AddOrEditShopInterfaceRequestModel requestModel);

        public Task<ShopInterfaceDTO> AddShopInterfaceAsync(int shopId, AddOrEditShopInterfaceRequestModel requestModel);

        public Task<ShopInterfaceDTO> FindShopInterfaceAsync(int shopId);
    }
}
