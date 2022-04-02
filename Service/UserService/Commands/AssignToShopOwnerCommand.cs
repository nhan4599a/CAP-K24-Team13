using MediatR;
using Shared.Models;
using System;

namespace UserService.Commands
{
    public class AssignToShopOwnerCommand : IRequest<CommandResponse<bool>>
    {
        public Guid UserId { get; set; }

        public int ShopId { get; set; }
    }
}
