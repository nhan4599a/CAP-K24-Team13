using MediatR;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;

namespace ShopInterfaceService.Commands
{
    public class CreateOrEditShopInterfaceCommand : IRequest<CommandResponse<ShopInterfaceDTO>>
    {
        public int ShopId { get; set; }

        public CreateOrEditInterfaceRequestModel RequestModel { get; set; }
    }
}
