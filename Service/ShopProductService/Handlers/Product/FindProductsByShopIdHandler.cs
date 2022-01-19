﻿using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared.DTOs;
using Shared.Models;
using ShopProductService.Commands.Product;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class FindProductsByShopIdHandler
        : IRequestHandler<FindProductsByShopIdQuery, PaginatedList<ProductDTO>>
    {
        private readonly IProductRepository _productRepository;

        public FindProductsByShopIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PaginatedList<ProductDTO>> Handle(FindProductsByShopIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _productRepository.GetAllProductsOfShopAsync(request.ShopId, request.PaginationInfo);
        }
    }
}