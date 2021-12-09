using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Product;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, CommandResponse<bool>>
    {
        private readonly IProductRepository _repository;

        public AddProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AddProductAsync(request.RequestModel);
        }
    }
}
