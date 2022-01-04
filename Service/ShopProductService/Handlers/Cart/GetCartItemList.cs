using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Cart;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Cart
{
    public class GetCartItemList : IRequestHandler<GetCartItemListQuery, List<CartItemDto>>
    {
        private readonly ICartRepository _cartRepository;

        public GetCartItemList(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public Task<List<CartItemDto>> Handle(GetCartItemListQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_cartRepository.GetCartAsync(request.UserId));
        }
    }
}
