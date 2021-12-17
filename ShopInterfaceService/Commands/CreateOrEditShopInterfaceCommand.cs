using MediatR;
using Shared;
using Shared.RequestModels;

namespace ShopInterfaceService.Commands
{
    public class CreateOrEditShopInterfaceCommand : IRequest<CommandResponse<int>>
    {
        public int ShopId { get; set; }

        public CreateOrEditShopInterfaceRequestModel RequestModel { get; set; }
    }
}
