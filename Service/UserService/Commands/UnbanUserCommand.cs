using MediatR;
using Shared.Models;
using System;

namespace UserService.Commands
{
    public class UnbanUserCommand : IRequest<CommandResponse<bool>>
    {
        public Guid UserId { get; set; }
    }
}
