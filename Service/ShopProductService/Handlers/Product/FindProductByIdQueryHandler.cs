﻿using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Product;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class FindProductByIdQueryHandler : IRequestHandler<FindProductByIdQuery, ProductDTO>, IDisposable
    {
        private readonly IProductRepository _repository;

        public FindProductByIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDTO> Handle(FindProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetProductAsync(request.Id);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}