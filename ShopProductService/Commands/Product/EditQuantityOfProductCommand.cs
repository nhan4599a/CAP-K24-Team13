using MediatR;
using Shared;
using Shared.DTOs;
using System;

namespace ShopProductService.Commands.Product
{
    public class EditQuantityOfProductCommand : IRequest<CommandResponse<ProductDTO>>
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
    }
}
