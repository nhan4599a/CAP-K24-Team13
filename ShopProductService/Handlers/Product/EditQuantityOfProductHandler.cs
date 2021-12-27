using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using Shared.DTOs;
using ShopProductService.Commands.Product;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class EditQuantityOfProductHandler : IRequestHandler<EditQuantityOfProductCommand, CommandResponse<ProductDTO>>
    {
        private readonly IProductRepository _repository;

        public EditQuantityOfProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<CommandResponse<ProductDTO>> Handle(EditQuantityOfProductCommand request, CancellationToken cancellationToken)
        {
            return await _repository.EditQuantity(request.Id, request.Quantity);
        }
    }
}
