using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Interfaces
{
    public interface ICategoryRepository  : IDisposable
    {
        Task<CategoryDTO> GetCategoryAsync(int id);

        Task<List<CategoryDTO>> GetAllCategoryAsync();

        Task<CommandResponse<bool>> AddCategoryAsync(AddOrEditCategoryRequestModel requestModel);

        Task<CommandResponse<bool>> EditCategoryAsync(int id, AddOrEditCategoryRequestModel requestModel);

        Task<CommandResponse<bool>> ActivateCategoryAsync(int id, bool isActivateCommand, bool shouldBeCascade);
    }
}
