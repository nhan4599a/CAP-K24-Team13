using AspNetCoreSharedComponent.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Models
{
    public record ExternalSignUpModel(string FirstName, string LastName, string Username, string Password,
        string RePassword, string Email,
        [ModelBinder(BinderType = typeof(StringToDateOnlyModelBinder))] DateOnly DoB,
        string Provider, string ProviderId, string ReturnUrl, string SessionId, string IdToken)
        : SignUpModel(FirstName, LastName, Username, Password, RePassword, Email, DoB)
    {
    }
}
