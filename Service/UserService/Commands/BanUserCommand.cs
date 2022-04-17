using MediatR;
using Shared.Models;
using System;

namespace UserService.Commands
{
    public class BanUserCommand : IRequest<CommandResponse<bool>>
    {
        public Guid UserId { get; set; }

        public uint? DayCount { get; set; }

        public string? Message { get; set; }
    }
}
