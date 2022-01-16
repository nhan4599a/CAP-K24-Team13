using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Models
{
    public record SignInModel(string Username, string Password, bool RememberMe, [FromQuery] string ReturnUrl)
        : AuthenticationModelBase(Username, Password)
    {
        public string ReturnUrl { get; init; } = string.IsNullOrEmpty(ReturnUrl) ? "~/" : ReturnUrl;
    }
}
