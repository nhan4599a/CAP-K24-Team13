using MediatR;
using Shared;
using System;

namespace ShopProductService.Commands.Product
{
    public class ActiveProductCommand : IRequest<CommandResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}
