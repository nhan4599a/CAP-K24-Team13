using System.ComponentModel.DataAnnotations;

namespace AuthServer.Models
{
    public record AuthenticationModelBase([Required] string Username, [Required] string Password);
}
