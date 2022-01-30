using MediatR;
using Shared;
using Shared.RequestModels;

namespace ShopProductService.Commands.Cart
{
    public class RemoveCartItemCommand : IRequest<CommandResponse<bool>>
    {
        public RemoveCartItemRequestModel requestModel { get; set; }
    }
}
