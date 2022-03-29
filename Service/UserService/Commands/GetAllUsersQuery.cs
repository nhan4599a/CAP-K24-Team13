using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace UserService.Commands
{
    public class GetAllUsersQuery : IRequest<PaginatedList<UserDTO>>
    {
        public int PageNumber { get; set; } = PaginationInfo.Default.PageNumber;

        public int PageSize { get; set; } = PaginationInfo.Default.PageSize;

        public bool Customer { get; set; }
    }
}
