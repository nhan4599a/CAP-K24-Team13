using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace ShopProductService.Commands.Product
{
    public class FindProductsByKeywordQuery : IRequest<PaginatedList<ProductDTO>>
    {
        public string Keyword { get; set; }

        public PaginationInfo PaginationInfo { get; set; } = PaginationInfo.Default;
    }
}
