using FluentValidation;
using Shared.RequestModels;

namespace ShopProductService.Validations
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
