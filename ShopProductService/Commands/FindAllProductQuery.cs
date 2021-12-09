using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace ShopProductService.Commands
{
    public class FindAllProductQuery : IRequest<List<ProductDTO>>
    {
    }
}
