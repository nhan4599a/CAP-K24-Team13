using MediatR;
using Shared;
using Shared.RequestModels;

namespace ShopInterfaceService.Commands
{
    public class CreateShopInterfaceCommand : IRequest<CommandResponse<bool>>
    {
        public CreateOrEditShopInterfaceRequestModel RequestModel { get; set; }
    }
}
