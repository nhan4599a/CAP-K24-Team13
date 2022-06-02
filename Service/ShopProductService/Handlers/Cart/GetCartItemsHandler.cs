using DatabaseAccessor.Repositories.Abstraction;
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
            var canParse = Guid.TryParse(request.UserId, out Guid parsedResult);
            if (!canParse)
            {
                throw new InvalidOperationException("UserId is invalid");
            }
            return await _cartRepository.GetCartAsync(parsedResult);
        }

        public void Dispose()
        {
            _cartRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
