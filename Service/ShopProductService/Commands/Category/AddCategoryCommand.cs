using MediatR;
using Shared;
using Shared.RequestModels;

namespace ShopProductService.Commands.Category
{
    public class AddCategoryCommand : IRequest<CommandResponse<bool>>
    {
        public int ShopId { get; set; }

        public CreateOrEditCategoryRequestModel RequestModel { get; set; }
    }
}
