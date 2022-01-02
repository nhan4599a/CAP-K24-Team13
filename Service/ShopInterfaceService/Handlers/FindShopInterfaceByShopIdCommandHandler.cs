using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using Shared.DTOs;
using ShopInterfaceService.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ShopInterfaceService.Handlers
{
    public class FindShopInterfaceByShopIdCommandHandler : 
        IRequestHandler<FindShopInterfaceByShopIdCommand, CommandResponse<ShopInterfaceDTO>>
    {
        private readonly IShopInterfaceRepository _repository;

        public FindShopInterfaceByShopIdCommandHandler(IShopInterfaceRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<ShopInterfaceDTO>> Handle(FindShopInterfaceByShopIdCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.FindShopInterfaceByShopIdAsync(request.ShopId);
        }
    }
}
