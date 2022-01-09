using System.ComponentModel.DataAnnotations;

namespace AuthServer.Models
{
    public record SignUpModel([Required] string Username, [Required] string Password,
            [Required] string Email, [Required] DateOnly DoB
        ) : AuthenticationModelBase(Username, Password);
}
