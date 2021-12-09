using MediatR;
using Shared;
using System;

namespace ShopProductService.Commands
{
    public class ActiveProductCommand : IRequest<CommandResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}
