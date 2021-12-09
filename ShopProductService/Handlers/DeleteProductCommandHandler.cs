using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, CommandResponse<bool>>
    {
        private readonly IProductRepository _repository;

        public DeleteProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteProductAsync(request.Id);
        }
    }
}
