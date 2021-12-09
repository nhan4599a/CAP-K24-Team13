using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace ShopProductService.Commands.Product
{
    public class FindProductsByKeywordQuery : IRequest<List<ProductDTO>>
    {
        public string Keyword { get; set; }
    }
}
