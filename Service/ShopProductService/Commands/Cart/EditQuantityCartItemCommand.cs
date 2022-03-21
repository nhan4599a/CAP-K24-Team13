using MediatR;
using Shared.Models;
using Shared.RequestModels;

namespace ShopProductService.Commands.Cart
{
    public class EditQuantityCartItemCommand : IRequest<CommandResponse<bool>>
    {
        public AddOrEditQuantityCartItemRequestModel requestModel { get; set; }
    }
}
