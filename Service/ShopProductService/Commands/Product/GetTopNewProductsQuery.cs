using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace ShopProductService.Commands.Product
{
    public class GetTopNewProductsQuery : IRequest<List<ProductDTO>>
    {
    }
}
