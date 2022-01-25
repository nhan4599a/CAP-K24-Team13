using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using ShopProductService.Commands.Cart;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Cart
{
    public class GetCartItemCountHandler : IRequestHandler<GetCartItemCountQuery, int>, IDisposable
    {
        private readonly ICartRepository _cartRepository;

        public GetCartItemCountHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<int> Handle(GetCartItemCountQuery request, CancellationToken cancellationToken)
        {
            return await _cartRepository.GetCartItemCountAsync(request.UserId);
        }

        public void Dispose()
        {
            _cartRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
