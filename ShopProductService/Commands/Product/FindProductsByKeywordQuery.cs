using MediatR;
using Shared;
using Shared.DTOs;

namespace ShopProductService.Commands.Product
{
    public class FindProductsByKeywordQuery : IRequest<PaginatedDataList<ProductDTO>>
    {
        public string Keyword { get; set; }

        public PaginationInfo PaginationInfo { get; set; }
    }
}
