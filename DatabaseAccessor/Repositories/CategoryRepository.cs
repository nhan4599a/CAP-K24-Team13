using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Shared;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;
        
        public CategoryRepository(ApplicationDbContext dbContext, Mapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> GetCategoryAsync(int id)
        {
            return _mapper.MapToCategoryDTO(await FindCategoryByIdAsync(id));
        }

        public async Task<PaginatedList<CategoryDTO>> GetAllCategoryAsync(PaginationInfo paginationInfo)
        {
            return await _dbContext.ShopCategories.AsNoTracking()
                .Select(category => _mapper.MapToCategoryDTO(category))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public async Task<CommandResponse<bool>> AddCategoryAsync(CreateOrEditCategoryRequestModel requestModel)
        {
            _dbContext.ShopCategories.Add(new ShopCategory().AssignByRequestModel(requestModel));
            try
            {
                await _dbContext.SaveChangesAsync();
                return CommandResponse<bool>.Success(true);
            }
            catch (Exception e)
            {
                return CommandResponse<bool>.Error(e.Message, e);
            }
        }

        public async Task<CommandResponse<bool>> EditCategoryAsync(int id, CreateOrEditCategoryRequestModel requestModel)
        {
            var category = await FindCategoryByIdAsync(id);
            if (category == null || category.IsDisabled)
                return CommandResponse<bool>.Error("Category is not found or already disabled", null);
            category.AssignByRequestModel(requestModel);
            _dbContext.Entry(category).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

        public async Task<CommandResponse<bool>> ActivateCategoryAsync(int id,
            bool isActivateCommand, bool shouldBeCascade)
        {
            var category = await FindCategoryByIdAsync(id);
            if (category == null)
                return CommandResponse<bool>.Error("Category is not found", null);
            if (isActivateCommand && !category.IsDisabled)
                return CommandResponse<bool>.Error("Category is already activated", null);
            if (!isActivateCommand && category.IsDisabled)
                return CommandResponse<bool>.Error("Category is already deactivated", null);
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            using var triggerSession = _dbContext.GetService<ITriggerService>().CreateSession(_dbContext);
            category.IsDisabled = !isActivateCommand;
            _dbContext.Entry(category).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            transaction.Commit();
            if (isActivateCommand && shouldBeCascade)
                await triggerSession.RaiseAfterCommitTriggers();
            return CommandResponse<bool>.Success(true);
        }

        private async Task<ShopCategory> FindCategoryByIdAsync(int id)
        {
            return await _dbContext.ShopCategories.FindAsync(id);
        }

        public async Task<PaginatedList<CategoryDTO>> GetCategoriesOfShopAsync(int shopId, PaginationInfo paginationInfo)
        {
            return await _dbContext.ShopCategories.AsNoTracking()
                .Where(category => category.ShopId == shopId)
                .Select(category => _mapper.MapToCategoryDTO(category))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
