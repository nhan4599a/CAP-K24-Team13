using Shared.DTOs;
using System.Threading.Tasks;

namespace ShopInterfaceService.Service
{
    public interface IShopService
    {
        public Task<ShopInterfaceDTO> EditShopInformation(ShopInterfaceDTO shopInterfaceDTO);
    }
}
