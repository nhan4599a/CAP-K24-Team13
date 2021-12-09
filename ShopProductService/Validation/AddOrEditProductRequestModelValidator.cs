using FluentValidation;
using Shared.RequestModels;

namespace ShopProductService.Validation
{
    public class AddOrEditProductRequestModelValidator : AbstractValidator<AddOrEditProductRequestModel>
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
