using MediatR;
using Shared;
using Shared.DTOs;

namespace ShopProductService.Commands.Product
{
    public class FindAllProductQuery : IRequest<PaginatedList<ProductDTO>>
    {
        public PaginationInfo PaginationInfo { get; set; }
    }
}
