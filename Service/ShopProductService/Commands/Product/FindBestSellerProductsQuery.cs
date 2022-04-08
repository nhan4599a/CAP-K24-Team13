using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace ShopProductService.Commands.Product
{
    public class FindBestSellerProductsQuery : IRequest<List<MinimalProductDTO>>
    {
        public int? ShopId { get; set; }
    }
}
