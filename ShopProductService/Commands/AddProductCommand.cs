using MediatR;
using Shared;
using Shared.RequestModels;

namespace ShopProductService.Commands
{
    public class AddProductCommand : IRequest<CommandResponse<bool>>
    {
        public AddOrEditProductRequestModel RequestModel { get; set; }
    }
}
