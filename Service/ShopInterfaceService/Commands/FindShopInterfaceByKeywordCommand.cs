using DatabaseAccessor.Contexts;
using MediatR;
using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopInterfaceService.Mediator
{
    public class FindShopInterfaceByKeywordCommand
    {
        public class Query : IRequest<ApiResult<List<ShopInterfaceDTO>>>
        {
            public string SearchString { get; set; }
        }
        public class Handler : IRequestHandler<Query, ApiResult<List<ShopInterfaceDTO>>>
        {
            private readonly ApplicationDbContext _applicationDbContext;
            public Handler(ApplicationDbContext applicationDbContext)
            {
                _applicationDbContext = applicationDbContext;
            }

            public Task<ApiResult<List<ShopInterfaceDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var shopList = _applicationDbContext.ShopInterfaces.Where(x => x.ShopName.Contains(request.SearchString)).ToList();
                if (shopList.Count == 0)
                {
                    return Task.FromResult(new ApiResult<List<ShopInterfaceDTO>> { Data = null, ResponseCode = 200 });
                }
                var shopDtoList = new List<ShopInterfaceDTO>();
                foreach (var shop in shopList)
                {
                    var shopDto = new ShopInterfaceDTO
                    {
                        Id = shop.Id,
                        Images = null,
                        ShopAddress = shop.ShopAddress,
                        ShopDescription = shop.ShopDescription,
                        ShopId = shop.ShopId,
                        ShopName = shop.ShopName,
                        ShopPhoneNumber = shop.ShopPhoneNumber
                    };
                    shopDtoList.Add(shopDto);
                };
                var result = new ApiResult<List<ShopInterfaceDTO>> { Data = shopDtoList, ResponseCode = 200 };
                return Task.FromResult(result);
            }
        }
    }
}
