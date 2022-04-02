using MediatR;
using Shared.DTOs;
using System;

namespace UserService.Commands
{
    public class GetUserByIdQuery : IRequest<UserDTO>
    {
        public Guid UserId { get; set; }
    }
}
