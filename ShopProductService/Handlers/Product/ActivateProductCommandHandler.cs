using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Product;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class ActivateProductCommandHandler : IRequestHandler<ActivateProductCommand, CommandResponse<bool>>
    {
        private readonly IProductRepository _repository;

        public ActivateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
        {
            return await _repository.ActivateProductAsync(request.Id, request.IsActivateCommand);
        }
    }
}
