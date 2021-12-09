using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Product;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class ActiveProductCommandHandler : IRequestHandler<ActiveProductCommand, CommandResponse<bool>>
    {
        private readonly IProductRepository _repository;

        public ActiveProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(ActiveProductCommand request, CancellationToken cancellationToken)
        {
            return await _repository.ActiveProductAsync(request.Id, false);
        }
    }
}
