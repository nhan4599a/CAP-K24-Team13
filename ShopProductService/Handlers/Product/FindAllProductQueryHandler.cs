using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Product;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class FindAllProductQueryHandler : IRequestHandler<FindAllProductQuery, List<ProductDTO>>
    {
        private readonly IProductRepository _repository;

        public FindAllProductQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductDTO>> Handle(FindAllProductQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllProductAsync();
            return products;
        }
    }
}
