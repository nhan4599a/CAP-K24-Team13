using MediatR;
using Shared;
using Shared.DTOs;

namespace ShopProductService.Commands.Category
{
    public class FindAllCategoryQuery : IRequest<PaginatedDataList<CategoryDTO>>
    {
        public PaginationInfo PaginationInfo { get; set; }
    }
}
