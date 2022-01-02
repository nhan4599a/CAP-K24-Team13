using MediatR;
using Shared;
using Shared.RequestModels;
using System;

namespace ShopProductService.Commands.Product
{
    public class CreateProductCommand : IRequest<CommandResponse<Guid>>
    {
        public CreateOrEditProductRequestModel RequestModel { get; set; }
    }
}
