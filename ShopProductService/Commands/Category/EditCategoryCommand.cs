using MediatR;
using Shared;
using Shared.RequestModels;

namespace ShopProductService.Commands.Category
{
    public class EditCategoryCommand : IRequest<CommandResponse<bool>>
    {
        public int Id { get; set; }

        public CreateOrEditCategoryRequestModel RequestModel { get; set; }
    }
}
