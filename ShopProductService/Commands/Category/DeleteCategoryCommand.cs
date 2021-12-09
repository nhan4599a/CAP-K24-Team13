using MediatR;
using Shared;

namespace ShopProductService.Commands.Category
{
    public class DeleteCategoryCommand : IRequest<CommandResponse<bool>>
    {
        public int Id { get; set; }
    }
}
