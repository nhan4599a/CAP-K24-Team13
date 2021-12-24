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
    public class FindProductsByKeywordQueryHandler : 
        IRequestHandler<FindProductsByKeywordQuery, PaginatedDataList<ProductDTO>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public FindProductsByKeywordQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedDataList<ProductDTO>> Handle(FindProductsByKeywordQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetProductsAsync(request.Keyword, request.PaginationInfo);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
