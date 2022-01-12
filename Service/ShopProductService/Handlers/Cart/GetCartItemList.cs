using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Cart;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Cart
{
    public class GetCartItemList : IRequestHandler<GetCartItemListQuery, CartDTO>
    {
        private readonly ICartRepository _cartRepository;

        public GetCartItemList(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<CartDTO> Handle(GetCartItemListQuery request, CancellationToken cancellationToken)
        {
            return await _cartRepository.GetCartAsync(request.UserId);
        }
    }
}
