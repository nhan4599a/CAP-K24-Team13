using FluentValidation;
using Shared.RequestModels;

namespace ShopInterfaceService.Validation
{
    public class CreateOrEditShopInterfaceRequestModelValidator : AbstractValidator<CreateOrEditShopInterfaceRequestModel>
    {
        public CreateOrEditShopInterfaceRequestModelValidator()
        {
            RuleFor(e => e.ShopAddress).NotNull().NotEmpty();
            RuleFor(e => e.ShopDescription).NotNull();
            RuleFor(e => e.ShopPhoneNumber).NotNull().NotEmpty();
        }
    }
}
