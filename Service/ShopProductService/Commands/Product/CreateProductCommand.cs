using MediatR;
using Shared.Models;
using Shared.RequestModels;
using System;

namespace ShopProductService.Commands.Product
{
    public class CreateProductCommand : IRequest<CommandResponse<Guid>>
    {
        public int ShopId { get; set; }

        public CreateOrEditProductRequestModel RequestModel { get; set; }
    }
}
