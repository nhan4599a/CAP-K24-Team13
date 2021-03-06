using FluentValidation;
using Shared.RequestModels;

namespace ShopProductService.Validations
{
    public class AddOrEditCategoryRequestModelValidator : AbstractValidator<CreateOrEditCategoryRequestModel>
    {
        public AddOrEditCategoryRequestModelValidator()
        {
            RuleFor(e => e.CategoryName).NotEmpty().NotNull();
            RuleFor(e => e.Special).NotEqual(0);
        }
    }
}
