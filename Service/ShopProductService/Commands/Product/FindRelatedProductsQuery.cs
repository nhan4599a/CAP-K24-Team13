using MediatR;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Collections.Generic;

namespace ShopProductService.Commands.Product
{
    public class FindRelatedProductsQuery : IRequest<CommandResponse<List<ProductDTO>>>
    {
        public Guid Id { get; set; }
    }
}
