using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared;
using Shared.DTOs;
using ShopInterfaceService.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopInterfaceService.Handlers
{
    public class CreateOrEditShopInterfaceCommandHandler :
        IRequestHandler<CreateOrEditShopInterfaceCommand, CommandResponse<ShopInterfaceDTO>>, IDisposable
    {
        private readonly IShopInterfaceRepository _repository;

        public CreateOrEditShopInterfaceCommandHandler(IShopInterfaceRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<ShopInterfaceDTO>> Handle(CreateOrEditShopInterfaceCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.EditShopInterfaceAsync(request.ShopId, request.RequestModel);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
