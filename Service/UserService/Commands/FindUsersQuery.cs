using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace UserService.Commands
{
    public class FindUsersQuery : IRequest<PaginatedList<UserDTO>>
    {
        public string Keyword { get; set; } = string.Empty;

        public int PageNumber { get; set; } = PaginationInfo.Default.PageNumber;

        public int PageSize { get; set; } = PaginationInfo.Default.PageSize;

        public string? RoleName { get; set; }
    }
}