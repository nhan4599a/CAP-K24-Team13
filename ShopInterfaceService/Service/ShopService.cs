using AutoMapper;
using DatabaseAccessor;
using DatabaseAccessor.Model;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using ShopInterfaceService.RequestModel;
using System.Threading.Tasks;
using System;
using Shared.Exception;

namespace ShopInterfaceService.Service
{
    public class ShopService : IShopService
    {
        private readonly ApplicationDbContext _dbContext;

        public ShopService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ShopInterfaceDTO> AddShopInterfaceAsync(int shopId, AddOrEditShopInterfaceRequestModel requestModel)
        {
            var shopInterfaceItem = new ShopInterface
            {
                ShopId = shopId,
                ShopAddress = requestModel.ShopAddress,
                ShopPhoneNumber = requestModel.ShopPhoneNumber,
                ShopDescription = requestModel.ShopDescription
            };
            _dbContext.ShopInterfaces.Add(shopInterfaceItem);
            await _dbContext.SaveChangesAsync();
            var result = ShopInterfaceDTO.FromSource(shopInterfaceItem);
            result.Id = shopInterfaceItem.Id;
            return result;
        }

        public async Task<ShopInterfaceDTO> EditShopInterfaceAsync(int shopId, AddOrEditShopInterfaceRequestModel requestModel)
        {
            var existedShopInterface = await FindShopInterfaceAsync(shopId);
            if (existedShopInterface == null)
                throw new ItemNotFoundException<int, ShopInterface>(shopId);
            //existedShopInterface.Images = shopInterfaceDTO.Images;
            await _dbContext.SaveChangesAsync();
            return existedShopInterface;
        }

        public async Task<ShopInterfaceDTO> FindShopInterfaceAsync(int shopId)
        {
            return ShopInterfaceDTO.FromSource(await _dbContext.ShopInterfaces.SingleOrDefaultAsync(e => e.ShopId == shopId)); 
        }
    }
}
