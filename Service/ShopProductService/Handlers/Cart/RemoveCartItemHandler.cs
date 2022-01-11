using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Cart;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Cart
{
    public class RemoveCartItemHandler : IRequestHandler<RemoveCartItemCommand, CommandResponse<bool>>
    {
        private readonly ICartRepository _cartRepository;

        public RemoveCartItemHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task<CommandResponse<bool>> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            return await _cartRepository.RemoveCartItem(request.requestModel);
        }
    }
}
