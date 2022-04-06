using Shared.DTOs;
using Shared.Models;
using System;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IUserRepository : IDisposable
    {
        Task<PaginatedList<UserDTO>> FindUsersAsync(string keyword, PaginationInfo paginationInfo, bool customer);

        Task<UserDTO> GetUserByIdAsync(Guid userId);

        Task<CommandResponse<bool>> ApplyBanAsync(Guid userId, uint? dayCount);

        Task<CommandResponse<bool>> UnbanAsync(Guid userId);

        Task<CommandResponse<bool>> AssignShopOwnerAsync(Guid userId, int shopId);
    }
}
