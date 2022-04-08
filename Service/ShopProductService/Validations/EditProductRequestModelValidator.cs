using FluentValidation;
using Shared.RequestModels;

namespace ShopProductService.Validations
{
    public class EditProductRequestModelValidator : AbstractValidator<EditProductRequestModel>
    {
        public EditProductRequestModelValidator()
        {
            RuleFor(e => e.ProductName).NotNull();
            RuleFor(e => e.Discount).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);
            RuleFor(e => e.Price).GreaterThan(0);
        }
    }
}
