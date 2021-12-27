using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using Shared.DTOs;
using ShopProductService.Commands.Product;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class FindAllProductQueryHandler : 
        IRequestHandler<FindAllProductQuery, PaginatedList<ProductDTO>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public FindAllProductQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<ProductDTO>> Handle(FindAllProductQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllProductAsync(request.PaginationInfo);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
