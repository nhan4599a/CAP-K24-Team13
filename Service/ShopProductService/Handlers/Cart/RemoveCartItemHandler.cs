using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Cart;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Cart
{
    public class RemoveCartItemHandler : IRequestHandler<RemoveCartItemCommand, CommandResponse<bool>>,
        IDisposable
    {
        private readonly ICartRepository _cartRepository;

        public RemoveCartItemHandler(ICartRepository repository)
        {
            _cartRepository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            return await _cartRepository.RemoveCartItemAsync(request.requestModel);
        }

        public void Dispose()
        {
            _cartRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
