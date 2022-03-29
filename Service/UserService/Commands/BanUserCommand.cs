using MediatR;
using Shared.Models;
using System;

namespace UserService.Commands
{
    public class BanUserCommand : IRequest<CommandResponse<bool>>
    {
        public Guid UserId { get; set; }

        public AccountPunishmentBehavior Behavior { get; set; }
    }
}
