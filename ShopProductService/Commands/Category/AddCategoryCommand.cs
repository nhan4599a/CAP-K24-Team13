using MediatR;
using Shared;
using Shared.RequestModels;

namespace ShopProductService.Commands.Category
{
    public class AddCategoryCommand : IRequest<CommandResponse<bool>>
    {
        public CreateOrEditCategoryRequestModel RequestModel { get; set; }
    }
}
