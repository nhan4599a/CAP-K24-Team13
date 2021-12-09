using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers
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
            return await _repository.ActiveProductAsync(request.Id);
        }
    }
}
