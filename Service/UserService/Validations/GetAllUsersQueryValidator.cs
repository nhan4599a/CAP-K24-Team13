using FluentValidation;
using UserService.Commands;

namespace UserService.Validations
{
    public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
    {
        public GetAllUsersQueryValidator()
        {
            RuleFor(e => e.PageNumber).GreaterThanOrEqualTo(1).When(e => e.PageSize >= 1);
        }
    }
}
