using AspNetCoreSharedComponent.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthServer.Models
{
    public record UserSignUpModel(string FirstName, string LastName, string Username, string Password, string Email, string PhoneNumber,
        [ModelBinder(BinderType = typeof(StringToDateOnlyModelBinder))] DateOnly DoB)
        : AuthenticationModelBase(Username, Password);
}
