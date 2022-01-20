using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Cart;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Cart
{
    public class GetCartItemsHandler : IRequestHandler<GetCartItemsQuery, List<CartItemDTO>>, IDisposable
    {
        private readonly ICartRepository _cartRepository;

        public GetCartItemsHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<List<CartItemDTO>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
        {
            return await _cartRepository.GetCartAsync(request.UserId);
        }

        public void Dispose()
        {
            _cartRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
