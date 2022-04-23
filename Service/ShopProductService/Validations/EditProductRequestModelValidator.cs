using FluentValidation;
using Shared.RequestModels;

namespace ShopProductService.Validations
{
    public class EditProductRequestModelValidator : AbstractValidator<EditProductRequestModel>
    {
        public EditProductRequestModelValidator()
        {
            RuleFor(e => e.ProductName).NotNull().WithMessage("Product name is required");
            RuleFor(e => e.Discount).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100).WithMessage("Discount must be between 0 - 100");
            RuleFor(e => e.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }
}
