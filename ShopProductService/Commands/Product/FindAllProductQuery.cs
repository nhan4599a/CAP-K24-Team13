using MediatR;
using Shared;
using Shared.DTOs;

namespace ShopProductService.Commands.Product
{
    public class FindAllProductQuery : IRequest<PaginatedDataList<ProductDTO>>
    {
        public PaginationInfo PaginationInfo { get; set; }
    }
}
