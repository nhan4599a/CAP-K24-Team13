using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopInterfaceService.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopInterfaceService.Handlers
{
    public class CreateShopInterfaceCommandHandler :
        IRequestHandler<CreateShopInterfaceCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public CreateShopInterfaceCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(CreateShopInterfaceCommand request, CancellationToken cancellationToken)
        {
            return await _repository.ActivateProductAsync(request.Id, request.IsActivateCommand);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
