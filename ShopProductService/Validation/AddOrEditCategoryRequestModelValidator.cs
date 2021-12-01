using FluentValidation;
using ShopProductService.RequestModel;

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
