using MediatR;
using Shared.Models;
using System;

namespace UserService.Commands
{
    public class AuthorizeUserCommand : IRequest<CommandResponse<bool>>
    {
        public Guid UserId { get; set; }

        public bool AuthorizeToAdmin { get; set; }

        public bool ToTeam5Admin { get; set; }
    }
}
