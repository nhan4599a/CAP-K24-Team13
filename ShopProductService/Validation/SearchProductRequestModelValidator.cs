using FluentValidation;
using Shared.RequestModels;

namespace ShopProductService.Validation
{
    public class SearchProductRequestModelValidator : AbstractValidator<SearchProductRequestModel>
    {
        public SearchProductRequestModelValidator()
        {
            RuleFor(e => e.PaginationInfo.PageNumber).Must(pageNumber =>
            {
                if (pageNumber.HasValue)
                    return pageNumber >= 1;
                return true;
            });
            RuleFor(e => e.PaginationInfo.PageSize).GreaterThanOrEqualTo(1);
        }
    }
}
