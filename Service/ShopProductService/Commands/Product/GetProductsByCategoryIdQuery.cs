using MediatR;
using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;

namespace ShopProductService.Commands.Product
{
    public class GetProductsByCategoryIdQuery : IRequest<PaginatedList<ProductDTO>>
    {
        public List<int> CategoryIds { get; set; }

        public PaginationInfo PaginationInfo { get; set; } = PaginationInfo.Default;
    }
}
