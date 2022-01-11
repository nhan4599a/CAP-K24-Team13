using MediatR;
using Shared;
using Shared.RequestModels;

namespace ShopProductService.Commands.Cart
{
    public class AddCartItemCommand : IRequest<CommandResponse<bool>>
    {
        public AddOrEditQuantityCartItemRequestModel requestModel { get; set; }
    }
}
