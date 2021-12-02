using AutoMapper;
using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System.Threading.Tasks;

namespace ShopInterfaceService.Service
{
    public class ShopService : IShopService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ShopService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<ShopInterfaceDTO> EditShopInformation(ShopInterfaceDTO shopInterfaceDTO)
        {
            var existedShopInterface = await _applicationDbContext.ShopInterfaces.FirstOrDefaultAsync(s => s.Id == shopInterfaceDTO.Id);
            if (existedShopInterface == null) return null;
            existedShopInterface.Option = shopInterfaceDTO.Option;
            existedShopInterface.Description = shopInterfaceDTO.Description;
            existedShopInterface.ShopId = shopInterfaceDTO.ShopId;
            existedShopInterface.Images = shopInterfaceDTO.Images;
            var result = await _applicationDbContext.SaveChangesAsync() > 0;
            return result ? shopInterfaceDTO : null;
        }
    }
}
