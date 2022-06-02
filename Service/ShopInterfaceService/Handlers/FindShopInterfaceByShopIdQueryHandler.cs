using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using Shared.Models;
using ShopInterfaceService.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopInterfaceService.Handlers
{
    public class FindShopInterfaceByShopIdQueryHandler : 
        IRequestHandler<FindShopInterfaceByShopIdQuery, CommandResponse<ShopInterfaceDTO>>, IDisposable
    {
        private readonly IShopInterfaceRepository _repository;

        public FindShopInterfaceByShopIdQueryHandler(IShopInterfaceRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<ShopInterfaceDTO>> Handle(FindShopInterfaceByShopIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.FindShopInterfaceByShopIdAsync(request.ShopId);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
