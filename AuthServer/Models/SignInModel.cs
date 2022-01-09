using System.ComponentModel.DataAnnotations;

namespace AuthServer.Models
{
    public record SignInModel([Required] string Username, [Required] string Password, bool RememberMe = false, string ReturnUrl = "~/")
        : AuthenticationModelBase(Username, Password);
}
