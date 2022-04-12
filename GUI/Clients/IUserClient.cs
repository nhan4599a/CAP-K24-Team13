using Refit;
using Shared.DTOs;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public interface IUserClient
    {
        [Get("/users/{userId}")]
        Task<ApiResponse<ApiResult<UserDTO>>> GetUserInfo(string userId);
    }
}
