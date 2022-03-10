using FluentValidation;
using OrderService.Commands;

namespace OrderService.Validations
{
    public class FindInvoiceQueryValidator : AbstractValidator<FindInvoiceQuery>
    {
        public FindInvoiceQueryValidator()
        {
            RuleFor(e => e.Key).NotEmpty();
            RuleFor(e => e.Value).NotEmpty();
            RuleFor(e => e.PageNumber).GreaterThanOrEqualTo(1).When(e => e.PageSize >= 1);
        }
    }
}
