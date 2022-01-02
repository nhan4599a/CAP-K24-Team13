using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Product;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class ActivateProductCommandHandler : 
        IRequestHandler<ActivateProductCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public ActivateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
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
