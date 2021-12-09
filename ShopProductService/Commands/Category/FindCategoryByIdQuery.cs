using MediatR;
using Shared.DTOs;

namespace ShopProductService.Commands.Category
{
    public class FindCategoryByIdQuery : IRequest<CategoryDTO>
    {
        public int Id { get; set; }
    }
}
