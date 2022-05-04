using FluentValidation;
using InvoiceService.Commands;

namespace InvoiceService.Validations
{
    public class FindInvoiceQueryValidator : AbstractValidator<FindInvoiceQuery>
    {
        public FindInvoiceQueryValidator()
        {
            RuleFor(e => e.PageNumber).GreaterThanOrEqualTo(1).When(e => e.PageSize >= 1);
        }
    }
}
