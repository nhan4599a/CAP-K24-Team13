using DatabaseAccessor.Contexts;
using MediatR;
using Shared;
using Shared.DTOs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopInterfaceService.Mediator
{
    public class FindShop
    {
        public class Query : IRequest<CommandResponse<ShopInterfaceDTO>>
        {
            public string SearchString { get; set; }
        }
        public class Handler : IRequestHandler<Query, CommandResponse<ShopInterfaceDTO>>
        {
            private readonly ApplicationDbContext _applicationDbContext;
            public Handler(ApplicationDbContext applicationDbContext)
            {
                _applicationDbContext = applicationDbContext;
            }

            public Task<CommandResponse<ShopInterfaceDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var shop = _applicationDbContext.ShopInterfaces.Where(x => x.ShopName.Contains(request.SearchString)).ToList();
                if (shop == null) return null;
                var shopDto = new ShopInterfaceDTO
                {
                    Id = shop[0].Id,
                    Images = new string[] { shop[0].Images },
                    ShopAddress = shop[0].ShopAddress,
                    ShopDescription = shop[0].ShopDescription,
                    ShopId = shop[0].ShopId,
                    ShopName = shop[0].ShopName,
                    ShopPhoneNumber = shop[0].ShopPhoneNumber
                };
                var result = CommandResponse<ShopInterfaceDTO>.Success(shopDto);
                return Task.FromResult(result);
            }
        }
    }
}
