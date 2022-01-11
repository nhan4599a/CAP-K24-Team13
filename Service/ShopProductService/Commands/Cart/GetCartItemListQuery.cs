using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace ShopProductService.Commands.Cart
{
    public class GetCartItemListQuery : IRequest<List<CartItemDto>>
    {
        public string UserId { get; set; }
    }
}
