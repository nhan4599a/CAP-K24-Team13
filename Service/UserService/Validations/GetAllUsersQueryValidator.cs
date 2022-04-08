using FluentValidation;
using UserService.Commands;

namespace UserService.Validations
{
    public class GetAllUsersQueryValidator : AbstractValidator<FindUsersQuery>
    {
        public GetAllUsersQueryValidator()
        {
            RuleFor(e => e.PageNumber).GreaterThanOrEqualTo(1).When(e => e.PageSize >= 1);
        }
    }
}
