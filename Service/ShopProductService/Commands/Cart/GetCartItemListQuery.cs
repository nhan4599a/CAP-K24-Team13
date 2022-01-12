using MediatR;
using Shared.DTOs;

namespace ShopProductService.Commands.Cart
{
    public class GetCartItemListQuery : IRequest<CartDTO>
    {
        public string UserId { get; set; }
    }
}
