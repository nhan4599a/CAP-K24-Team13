using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using Shared.RequestModels;
using ShopProductService.Commands.Cart;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Cart
{
    public class AddCartItemHandler : IRequestHandler<AddCartItemCommand, CommandResponse<bool>>
    {
        private readonly ICartRepository _cartRepository;

        public AddCartItemHandler(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }      

        public async Task<CommandResponse<bool>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var result = await _cartRepository.AddProductToCart(request.requestModel);
            return result;
        }
    }
}
