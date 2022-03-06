using FluentValidation;
using Shared.RequestModels;

namespace ShopProductService.Validations
{
    public class SearchProductRequestModelValidator : AbstractValidator<SearchRequestModel>
    {
        public SearchProductRequestModelValidator()
        {
            RuleFor(e => e.PageNumber).GreaterThanOrEqualTo(1);
        }
    }
}
