using MediatR;
using Shared.Models;
using Shared.RequestModels;

namespace ShopProductService.Commands.Cart
{
    public class RemoveCartItemCommand : IRequest<CommandResponse<bool>>
    {
        public RemoveCartItemRequestModel requestModel { get; set; }
    }
}
