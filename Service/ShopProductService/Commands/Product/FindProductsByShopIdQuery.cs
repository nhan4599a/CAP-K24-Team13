using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace ShopProductService.Commands.Product
{
    public class FindProductsByShopIdQuery : IRequest<PaginatedList<ProductDTO>>
    {
        public int ShopId { get; set; }

        public PaginationInfo PaginationInfo { get; set; } = PaginationInfo.Default;
    }
}
