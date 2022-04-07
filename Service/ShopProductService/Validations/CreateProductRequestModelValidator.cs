using FluentValidation;
using Shared.RequestModels;

namespace ShopProductService.Validations
{
    public class CreateProductRequestModelValidator : AbstractValidator<CreateProductRequestModel>
    {
        public CreateProductRequestModelValidator()
        {
            Include(new EditProductRequestModelValidator());
            RuleFor(e => e.Quantity).GreaterThan(0);
        }
    }
}
