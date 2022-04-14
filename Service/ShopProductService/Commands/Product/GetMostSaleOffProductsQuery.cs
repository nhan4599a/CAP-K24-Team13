using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace ShopProductService.Commands.Product
{
    public class GetMostSaleOffProductsQuery : IRequest<List<MinimalProductDTO>>
    {
    }
}
