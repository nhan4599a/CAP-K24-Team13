using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace ShopProductService.Commands.Product
{
    public class FindAllProductQuery : IRequest<PaginatedList<ProductDTO>>
    {
        public PaginationInfo PaginationInfo { get; set; } = PaginationInfo.Default;

        public bool IncludeComments { get; set; } = true;
    }
}
