using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using ShopInterfaceService.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopInterfaceService.Handlers
{
    public class GetShopInterfacesQueryHandler
        : IRequestHandler<GetShopInterfacesQuery, Dictionary<int, ShopInterfaceDTO>>, IDisposable
    {
        private readonly IShopInterfaceRepository _repository;

        public GetShopInterfacesQueryHandler(IShopInterfaceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<int, ShopInterfaceDTO>> Handle(GetShopInterfacesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetShopInterfacesAsync(request.ShopIds);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
