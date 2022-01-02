using FluentValidation;
using Shared.RequestModels;

namespace ShopProductService.Validations
{
    public class AddOrEditProductRequestModelValidator : AbstractValidator<CreateOrEditProductRequestModel>
    {
        public AddOrEditProductRequestModelValidator()
        {
            RuleFor(e => e.ProductName).NotNull();
            RuleFor(e => e.Discount).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);
            RuleFor(e => e.Price).GreaterThan(0);
            RuleFor(e => e.Quantity).GreaterThan(0);
        }
    }
}
