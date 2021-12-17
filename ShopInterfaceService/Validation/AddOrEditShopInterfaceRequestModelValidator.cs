using FluentValidation;
using Shared.RequestModels;

namespace ShopInterfaceService.Validation
{
    public class AddOrEditShopInterfaceRequestModelValidator : AbstractValidator<CreateOrEditShopInterfaceRequestModel>
    {
        public AddOrEditShopInterfaceRequestModelValidator()
        {
            RuleFor(e => e.ShopAddress).NotNull().NotEmpty();
            RuleFor(e => e.ShopDescription).NotNull();
            RuleFor(e => e.ShopPhoneNumber).NotNull().NotEmpty();
        }
    }
}
