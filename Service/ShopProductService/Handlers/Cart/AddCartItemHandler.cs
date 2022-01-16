using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Cart;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Cart
{
    public class AddCartItemHandler : IRequestHandler<AddCartItemCommand, CommandResponse<bool>>,
        IDisposable
    {
        private readonly ICartRepository _cartRepository;

        public AddCartItemHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CommandResponse<bool>> Handle(
            AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartRepository.AddProductToCartAsync(request.RequestModel);
            return result;
        }

        public void Dispose()
        {
            _cartRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
