using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared;
using ShopProductService.Commands.Product;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class UpdateQuantityHandler : IRequestHandler<UpdateQuantityCommand, CommandResponse<int>>, IDisposable
    { 
        
            private readonly IProductRepository _repository;

            public UpdateQuantityHandler(IProductRepository repository)
            {
                _repository = repository;
            }

            public async Task<CommandResponse<int>> Handle(UpdateQuantityCommand request, CancellationToken cancellationToken)
            {
                return await _repository.UpdateQuantityAsync(request.ProductId, request.Quantity);
            }

            public void Dispose()
            {
                _repository.Dispose();
                GC.SuppressFinalize(this);
            }
        }
    
}
