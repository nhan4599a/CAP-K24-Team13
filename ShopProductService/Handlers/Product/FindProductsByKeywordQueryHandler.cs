using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Product;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class FindProductsByKeywordQueryHandler : 
        IRequestHandler<FindProductsByKeywordQuery, List<ProductDTO>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public FindProductsByKeywordQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductDTO>> Handle(FindProductsByKeywordQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetProductsAsync(request.Keyword);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
