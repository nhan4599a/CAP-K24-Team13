using DatabaseAccessor;
using DatabaseAccessor.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShopInterfaceService.Mediator
{
    public class AddOrEditShop
    {
        public class Command : IRequest<CommandResponse<ShopInterfaceDTO>>
        {
            public ShopInterfaceDTO shopInterfaceDTO { get; set; }
        }
        public class Handler : IRequestHandler<Command, CommandResponse<ShopInterfaceDTO>>
        {
            private readonly ApplicationDbContext _context;
            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }


            public async Task<CommandResponse<ShopInterfaceDTO>> Handle(Command request, CancellationToken cancellationToken)
            {
                var shop = new ShopInterface
                {
                    Images = request.shopInterfaceDTO.Images.ToString(),
                    ShopId = request.shopInterfaceDTO.ShopId,
                    ShopAddress = request.shopInterfaceDTO.ShopAddress,
                    ShopPhoneNumber = request.shopInterfaceDTO.ShopPhoneNumber,
                    ShopDescription = request.shopInterfaceDTO.ShopDescription,
                    ShopName = request.shopInterfaceDTO.ShopName,
                };
                var shopExisted = await _context.ShopInterfaces.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.shopInterfaceDTO.Id);
                var result = 0;
                if (shopExisted == null)
                {
                    await _context.ShopInterfaces.AddAsync(shop);                    
                }
                else
                {
                    shop.Id = shopExisted.Id;
                    shopExisted = shop;
                    _context.ShopInterfaces.Update(shopExisted);
                }
                result = await _context.SaveChangesAsync();
                return result > 0 ? new CommandResponse<ShopInterfaceDTO> { Response = request.shopInterfaceDTO} : null;
            }
        }
    }
}
