using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace ShopInterfaceService.Commands
{
    public class GetShopInterfacesQuery : IRequest<Dictionary<int, ShopInterfaceDTO>>
    {
        public List<int> ShopIds { get; set; }
    }
}
