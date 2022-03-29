using Shared.DTOs;
using Shared.Models;
using System;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IUserRepository : IDisposable
    {
        Task<PaginatedList<UserDTO>> GetAllUsersAsync(PaginationInfo paginationInfo);
    }
}
