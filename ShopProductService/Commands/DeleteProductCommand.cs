using MediatR;
using Shared;
using System;

namespace ShopProductService.Commands
{
    public class DeleteProductCommand : IRequest<CommandResponse<bool>>
    {
        public Guid Id { get; set; }
    }
}
