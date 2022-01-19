using FluentValidation;
using Shared.RequestModels;

namespace ShopProductService.Validations
{
    public class SearchProductRequestModelValidator : AbstractValidator<SearchRequestModel>
    {
        public SearchProductRequestModelValidator()
        {
            RuleFor(e => e.PaginationInfo.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(e => e.PaginationInfo.PageSize).Must(pageSize =>
            {
                if (pageSize.HasValue)
                    return pageSize >= 1;
                return true;
            });
        }
    }
}
