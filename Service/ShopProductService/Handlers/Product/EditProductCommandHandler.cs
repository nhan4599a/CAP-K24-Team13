using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using Shared.Models;
using ShopProductService.Commands.Product;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class EditProductCommandHandler : IRequestHandler<EditProductCommand, CommandResponse<ProductDTO>>,
        IDisposable
    {
        private readonly IProductRepository _repository;

        public EditProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<ProductDTO>> Handle(EditProductCommand request, CancellationToken cancellationToken)
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
