using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using ShopProductService.Commands.Cart;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Cart
{
    public class EditQuantityCartItemHandler : 
        IRequestHandler<EditQuantityCartItemCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly ICartRepository _cartRepository;

        public EditQuantityCartItemHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CommandResponse<bool>> Handle(EditQuantityCartItemCommand request, CancellationToken cancellationToken)
        {
            return await _cartRepository.EditQuantityAsync(request.requestModel);
        }

        public void Dispose()
        {
            _cartRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
