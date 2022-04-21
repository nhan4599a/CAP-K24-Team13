using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Product;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class GetTopNewProductsQueryHandler : IRequestHandler<GetTopNewProductsQuery, List<ProductDTO>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public GetTopNewProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductDTO>> Handle(GetTopNewProductsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetTopNewsProductsAsync();
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
