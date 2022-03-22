using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace ShopInterfaceService.Commands
{
    public class FindShopInterfaceByShopIdQuery : IRequest<CommandResponse<ShopInterfaceDTO>>
    {
        public int ShopId { get; set; }
    }
}
