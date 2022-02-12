using MediatR;
using Shared.DTOs;
using System;

namespace ShopProductService.Commands.Product
{
    public class FindProductByIdQuery : IRequest<MinimalProductDTO>
    {
        public Guid Id { get; set; }

        public bool IsMinimal { get; set; } = false;
    }
}
