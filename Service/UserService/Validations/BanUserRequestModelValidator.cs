using FluentValidation;
using UserService.RequestModels;

namespace UserService.Validations
{
    public class BanUserRequestModelValidator : AbstractValidator<BanUserRequestModel>
    {
        public BanUserRequestModelValidator()
        {
            RuleFor(e => e.DayCount).Must(value =>
            {
                if (value.HasValue && value.Value == 0)
                    return false;
                return true;
            });
        }
    }
}
