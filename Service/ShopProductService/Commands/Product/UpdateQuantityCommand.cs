using MediatR;
using Shared;
using System;

namespace ShopProductService.Commands.Product
{
    public class UpdateQuantityCommand : IRequest<CommandResponse<int>>
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
