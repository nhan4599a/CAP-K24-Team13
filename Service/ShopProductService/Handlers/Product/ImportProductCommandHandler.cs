using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using ShopProductService.Commands.Product;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class ImportProductCommandHandler
        : IRequestHandler<ImportProductCommand, CommandResponse<int>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public ImportProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<int>> Handle(ImportProductCommand request, CancellationToken cancellationToken)
        {
            return await _repository.ImportProductQuantityAsync(request.ProductId, request.Quantity);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }

}
