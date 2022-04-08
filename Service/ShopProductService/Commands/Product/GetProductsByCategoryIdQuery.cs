using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace ShopProductService.Commands.Product
{
    public class GetProductsByCategoryIdQuery : IRequest<PaginatedList<ProductDTO>>
    {
        public int CategoryId { get; set; }

        public PaginationInfo PaginationInfo { get; set; } = PaginationInfo.Default;
    }
}
