using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using ShopProductService.Commands.Product;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, CommandResponse<Guid>>,
        IDisposable
    {
        private readonly IProductRepository _repository;

        public CreateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AddProductAsync(request.RequestModel);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
