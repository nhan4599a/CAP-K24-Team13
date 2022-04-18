using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace ShopProductService.Commands.Product
{
    public abstract class FindProductsQuery : IRequest<PaginatedList<ProductDTO>>
    {
        public PaginationInfo PaginationInfo { get; set; } = PaginationInfo.Default;

        public bool IncludeFilter { get; set; } = true;
    }
}
