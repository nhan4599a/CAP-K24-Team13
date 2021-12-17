using MediatR;
using Shared;
using Shared.RequestModels;

namespace ShopProductService.Commands.Product
{
    public class CreateProductCommand : IRequest<CommandResponse<bool>>
    {
        public CreateOrEditProductRequestModel RequestModel { get; set; }
    }
}
