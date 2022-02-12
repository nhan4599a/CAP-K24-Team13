using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Product;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class FindProductByIdQueryHandler : 
        IRequestHandler<FindProductByIdQuery, MinimalProductDTO>, IDisposable
    {
        private readonly IProductRepository _repository;

        public FindProductByIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<MinimalProductDTO> Handle(FindProductByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.IsMinimal)
                return await _repository.GetMinimalProductAsync(request.Id);
            return await _repository.GetProductAsync(request.Id);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
