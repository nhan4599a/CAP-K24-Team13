using AuthServer.Models;
using FluentValidation;

namespace AuthServer.Validators
{
    public class EditUserInformationModelValidator : AbstractValidator<EditUserInformationModel>
    {
        public EditUserInformationModelValidator()
        {
            RuleFor(model => model.PhoneNumber)
                .Matches(@"0\d{9}")
                .WithMessage("Phone must be in a valid phone format");
        }
    }
}
