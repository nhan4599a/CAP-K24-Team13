using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using Shared.Models;
using ShopProductService.Commands.Product;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class FindRelatedProductsQueryHandler
        : IRequestHandler<FindRelatedProductsQuery, CommandResponse<List<ProductDTO>>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public async Task<CommandResponse<List<ProductDTO>>> Handle(FindRelatedProductsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetRelatedProductsAsync(request.Id);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
