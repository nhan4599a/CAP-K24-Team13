using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace ShopProductService.Commands.Cart
{
    public class GetCartItemsQuery : IRequest<List<CartItemDTO>>
    {
        public string UserId { get; set; }
    }
}
