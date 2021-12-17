using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class ShopInterfaceRepository : IShopInterfaceRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public ShopInterfaceRepository(ApplicationDbContext dbContext, Mapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ShopInterfaceDTO> FindShopInterfaceByShopId(int shopId)
        {
            var shopInterface = await _dbContext.ShopInterfaces
                .AsNoTracking().FirstOrDefaultAsync(e => e.ShopId == shopId);
            return _mapper.MapToShopInterfaceDTO(shopInterface);
        }

        public async Task<CommandResponse<int>> EditShopInterfaceAsync(int shopId,
            CreateOrEditShopInterfaceRequestModel requestModel)
        {
            var shopInterface = await _dbContext.ShopInterfaces.FirstOrDefaultAsync(e => e.ShopId == shopId);
            if (shopInterface == null)
            {
                shopInterface = new ShopInterface().AssignByRequestModel(requestModel);
                _dbContext.ShopInterfaces.Add(shopInterface);
            }
            else
            {
                shopInterface.AssignByRequestModel(requestModel);
                _dbContext.Entry(shopInterface).State = EntityState.Modified;
            }
            await _dbContext.SaveChangesAsync();
            return new CommandResponse<int> { Response = shopInterface.Id };
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}