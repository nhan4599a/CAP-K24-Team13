using Shared;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface ICategoryRepository  : IDisposable
    {
        Task<CategoryDTO> GetCategoryAsync(int id);

        Task<PaginatedList<CategoryDTO>> GetAllCategoriesAsync(PaginationInfo paginationInfo);

        Task<CommandResponse<bool>> AddCategoryAsync(int shopId, CreateOrEditCategoryRequestModel requestModel);

        Task<CommandResponse<bool>> EditCategoryAsync(int categoryId, CreateOrEditCategoryRequestModel requestModel);

        Task<CommandResponse<bool>> ActivateCategoryAsync(int categoryId, bool isActivateCommand, bool shouldBeCascade);

        Task<PaginatedList<CategoryDTO>> GetCategoriesOfShopAsync(int shopId, PaginationInfo paginationInfo);
    }
}
