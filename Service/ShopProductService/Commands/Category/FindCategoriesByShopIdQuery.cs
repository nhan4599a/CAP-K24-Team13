using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace ShopProductService.Commands.Category
{
    public class FindCategoriesByShopIdQuery : IRequest<PaginatedList<CategoryDTO>>
    {
        public int ShopId { get; set; }

        public PaginationInfo PaginationInfo { get; set; } = PaginationInfo.Default;
    }
}
