using MediatR;

namespace ShopProductService.Commands.Cart
{
    public class GetCartItemCountQuery : IRequest<int>
    {
        public string UserId { get; set; }
    }
}
