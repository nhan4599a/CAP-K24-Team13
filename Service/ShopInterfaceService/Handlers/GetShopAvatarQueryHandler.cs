using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using ShopInterfaceService.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopInterfaceService.Handlers
{
    public class GetShopAvatarQueryHandler : IRequestHandler<GetShopAvatarQuery, Dictionary<int, string>>, IDisposable
    {
        private readonly IShopInterfaceRepository _repository;

        public GetShopAvatarQueryHandler(IShopInterfaceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<int, string>> Handle(GetShopAvatarQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetShopAvatar(request.ShopIds);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
