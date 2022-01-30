using MediatR;
using Shared;
using Shared.RequestModels;

namespace ShopProductService.Commands.Cart
{
    public class EditQuantityCartItemCommand : IRequest<CommandResponse<bool>>
    {
        public AddOrEditQuantityCartItemRequestModel requestModel { get; set; }
    }
}
