using Shared;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Interfaces
{
    public interface ICategoryRepository  : IDisposable
    {
        Task<CategoryDTO> GetCategoryAsync(int id);

        Task<PaginatedList<CategoryDTO>> GetAllCategoryAsync(PaginationInfo paginationInfo);

        Task<CommandResponse<bool>> AddCategoryAsync(CreateOrEditCategoryRequestModel requestModel);

        Task<CommandResponse<bool>> EditCategoryAsync(int id, CreateOrEditCategoryRequestModel requestModel);

        Task<CommandResponse<bool>> ActivateCategoryAsync(int id, bool isActivateCommand, bool shouldBeCascade);
    }
}
