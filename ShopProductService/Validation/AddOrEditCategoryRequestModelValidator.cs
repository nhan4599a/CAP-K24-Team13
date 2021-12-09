using FluentValidation;
using Shared.RequestModels;

namespace ShopProductService.Validation
{
    public class AddOrEditCategoryRequestModelValidator : AbstractValidator<AddOrEditCategoryRequestModel>
    {
        public AddOrEditCategoryRequestModelValidator()
        {
            RuleFor(e => e.CategoryName).NotNull();
            RuleFor(e => e.Special).NotEqual(0);
        }
    }
}
