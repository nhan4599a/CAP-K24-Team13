using MediatR;
using Shared.Models;
using Shared.RequestModels;

namespace ShopProductService.Commands.Cart
{
    public class AddCartItemCommand : IRequest<CommandResponse<bool>>
    {
        public AddOrEditQuantityCartItemRequestModel RequestModel { get; set; }
    }
}
