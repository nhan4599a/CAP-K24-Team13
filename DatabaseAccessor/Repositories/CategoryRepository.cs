using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
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

        public async Task<List<CategoryDTO>> GetAllCategoryAsync()
        {
            return await _dbContext.ShopCategories.AsNoTracking()
                .Select(category => _mapper.MapToCategoryDTO(category)).ToListAsync();
        }

        public async Task<CommandResponse<bool>> AddCategoryAsync(AddOrEditCategoryRequestModel requestModel)
        {
            _dbContext.ShopCategories.Add(new ShopCategory().AssignByRequestModel(requestModel));
            try
            {
                await _dbContext.SaveChangesAsync();
                return new CommandResponse<bool> { Response = true };
            }
            catch (Exception e)
            {
                return new CommandResponse<bool> { Response = false, ErrorMessage = e.Message, Exception = e };
            }
        }

        public async Task<CommandResponse<bool>> EditCategoryAsync(int id, AddOrEditCategoryRequestModel requestModel)
        {
            var category = await FindCategoryByIdAsync(id);
            if (category == null || category.IsDisabled)
                return new CommandResponse<bool> { Response = false, ErrorMessage = "Category is not found or already disabled" };
            category.AssignByRequestModel(requestModel);
            _dbContext.Entry(category).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return new CommandResponse<bool> { Response = true };
        }

        public async Task<CommandResponse<bool>> ActiveCategoryAsync(int id, bool shouldDisable)
        {
            var category = await FindCategoryByIdAsync(id);
            if (category == null || category.IsDisabled == shouldDisable)
                return new CommandResponse<bool> { Response = false, ErrorMessage = "Category is not found or already enabled/disabled" };
            category.IsDisabled = shouldDisable;
            _dbContext.Entry(category).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return new CommandResponse<bool> { Response = true };
        }

        private async Task<ShopCategory> FindCategoryByIdAsync(int id)
        {
            return await _dbContext.ShopCategories.FindAsync(id);
        }
    }
}
