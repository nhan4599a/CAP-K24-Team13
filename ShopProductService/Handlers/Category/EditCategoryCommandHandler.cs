using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Category;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class EditCategoryCommandHandler : 
        IRequestHandler<EditCategoryCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly ICategoryRepository _repository;

        public EditCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _repository.EditCategoryAsync(request.Id, request.RequestModel);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
