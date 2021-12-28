using MediatR;
using Shared;
using Shared.DTOs;

namespace ShopProductService.Commands.Category
{
    public class FindAllCategoryQuery : IRequest<PaginatedList<CategoryDTO>>
    {
        public PaginationInfo PaginationInfo { get; set; }
    }
}
