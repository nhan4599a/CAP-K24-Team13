using MediatR;
using Shared;
using Shared.DTOs;

namespace ShopInterfaceService.Commands
{
    public class FindShopInterfaceByShopIdCommand : IRequest<CommandResponse<ShopInterfaceDTO>>
    {
        public int ShopId { get; set; }
    }
}
