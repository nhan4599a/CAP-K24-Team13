using MediatR;
using Shared;

namespace ShopProductService.Commands.Category
{
    public class ActiveCategoryCommand : IRequest<CommandResponse<bool>>
    {
        public int Id { get; set; }
    }
}
