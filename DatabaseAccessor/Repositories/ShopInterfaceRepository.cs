using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System;
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

        public async Task<CommandResponse<ShopInterfaceDTO>> FindShopInterfaceByShopIdAsync(int shopId)
        {
            var shopInterface = await _dbContext.ShopInterfaces
                .AsNoTracking().FirstOrDefaultAsync(e => e.ShopId == shopId);
            return CommandResponse<ShopInterfaceDTO>.Success(_mapper.MapToShopInterfaceDTO(shopInterface));
        }

        public async Task<CommandResponse<ShopInterfaceDTO>> EditShopInterfaceAsync(int shopId,
            CreateOrEditInterfaceRequestModel requestModel)
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
            try
            {
                await _dbContext.SaveChangesAsync();
            } catch (Exception ex)
            {
                return CommandResponse<ShopInterfaceDTO>.Error(ex.Message, ex);
            }
            return CommandResponse<ShopInterfaceDTO>.Success(_mapper.MapToShopInterfaceDTO(shopInterface));
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}