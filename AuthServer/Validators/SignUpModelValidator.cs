﻿using AuthServer.Configurations;
using AuthServer.Models;
using FluentValidation;

namespace AuthServer.Validators
{
    public class SignUpModelValidator : AbstractValidator<SignUpModel>
    {
        public SignUpModelValidator()
        {
            RuleFor(model => model.Email).EmailAddress()
                .WithMessage("Email must be in a valid email format");
            RuleFor(model => model.RePassword).Must((model, field) => field == model.Password)
                .WithMessage("Re input password must be equal to password");
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var maxAcceptedDate = currentDate.AddYears(-AccountConfig.MinAge);
            var minAcceptedDate = currentDate.AddYears(-AccountConfig.MaxAge);
            RuleFor(model => model.DoB).Must(DoB => DoB <= maxAcceptedDate && DoB >= minAcceptedDate)
                .WithMessage($"Your age must be between {AccountConfig.MinAge} and {AccountConfig.MaxAge}" +
                $" (from {minAcceptedDate:dd/MM/yyyy} to {maxAcceptedDate:dd/MM/yyyy})");
        }
    }
}
