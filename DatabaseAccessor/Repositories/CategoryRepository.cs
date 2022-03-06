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

        public async Task<CategoryDTO> GetCategoryAsync(int categoryId)
        {
            return _mapper.MapToCategoryDTO(await FindCategoryByIdAsync(categoryId));
        }

        public async Task<PaginatedList<CategoryDTO>> GetAllCategoriesAsync(PaginationInfo paginationInfo)
        {
            return await _dbContext.ShopCategories.AsNoTracking()
                .Select(category => _mapper.MapToCategoryDTO(category))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public async Task<CommandResponse<bool>> AddCategoryAsync(int shopId, CreateOrEditCategoryRequestModel requestModel)
        {
            var shopCategory = new ShopCategory().AssignByRequestModel(requestModel);
            shopCategory.ShopId = shopId;
            if (await _dbContext.ShopCategories.AnyAsync(category => category.ShopId == shopCategory.ShopId
                && category.CategoryName == shopCategory.CategoryName))
            {
                return CommandResponse<bool>.Error("Category's name is already existed", null);
            }
            _dbContext.ShopCategories.Add(shopCategory);
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

        public async Task<CommandResponse<bool>> EditCategoryAsync(int categoryId, CreateOrEditCategoryRequestModel requestModel)
        {
            var category = await FindCategoryByIdAsync(categoryId);
            if (category == null || category.IsDisabled)
                return CommandResponse<bool>.Error("Category is not found or already disabled", null);
            category.AssignByRequestModel(requestModel);
            _dbContext.Entry(category).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

        public async Task<CommandResponse<bool>> ActivateCategoryAsync(int categoryId,
            bool isActivateCommand, bool shouldBeCascade)
        {
            var category = await FindCategoryByIdAsync(categoryId);
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

        private async Task<ShopCategory> FindCategoryByIdAsync(int categoryId)
        {
            return await _dbContext.ShopCategories.FindAsync(categoryId);
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
