using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared;
using ShopProductService.Commands.Category;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly ICategoryRepository _repository;

        public AddCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AddCategoryAsync(request.ShopId, request.RequestModel);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
