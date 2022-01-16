using AuthServer.Configurations;
using AuthServer.Models;
using FluentValidation;

namespace AuthServer.Validators
{
    public class ExternalSignUpModelValidator : AbstractValidator<ExternalSignUpModel>
    {
        public ExternalSignUpModelValidator()
        {
            Include(new SignUpModelValidator());
        }
    }
}
