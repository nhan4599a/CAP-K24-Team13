using MediatR;
using Shared.Models;
using Shared.RequestModels;

namespace ShopProductService.Commands.Category
{
    public class AddCategoryCommand : IRequest<CommandResponse<bool>>
    {
        public int ShopId { get; set; }

        public CreateOrEditCategoryRequestModel RequestModel { get; set; }
    }
}
