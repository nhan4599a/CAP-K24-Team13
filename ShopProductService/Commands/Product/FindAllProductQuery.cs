using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace ShopProductService.Commands.Product
{
    public class FindAllProductQuery : IRequest<List<ProductDTO>>
    {
    }
}
