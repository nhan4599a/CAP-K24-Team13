using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Product;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class EditProductCommandHandler : IRequestHandler<EditProductCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public EditProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            return await _repository.EditProductAsync(request.Id, request.RequestModel);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
