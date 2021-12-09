using MediatR;
using Shared;
using Shared.RequestModels;
using System;

namespace ShopProductService.Commands.Product
{
    public class EditProductCommand : IRequest<CommandResponse<bool>>
    {
        public Guid Id { get; set; }

        public AddOrEditProductRequestModel RequestModel { get; set; }
    }
}
