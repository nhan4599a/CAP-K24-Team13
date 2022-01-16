using AspNetCoreSharedComponent.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Models
{
    public record SignUpModel(string FirstName, string LastName, string Username, string Password,
        string RePassword, string Email,
        [ModelBinder(BinderType = typeof(StringToDateOnlyModelBinder))] DateOnly DoB)
        : AuthenticationModelBase(Username, Password);
}
