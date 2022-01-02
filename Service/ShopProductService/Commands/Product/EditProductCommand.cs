using MediatR;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;

namespace ShopProductService.Commands.Product
{
    public class EditProductCommand : IRequest<CommandResponse<ProductDTO>>
    {
        public Guid Id { get; set; }

        public CreateOrEditProductRequestModel RequestModel { get; set; }
    }
}
