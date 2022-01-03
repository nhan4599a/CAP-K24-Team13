namespace AuthServer.Models
{
    public class SignUpModel : AuthenticationModelBase
    {
        public string Email { get; set; }

        public DateOnly DoB { get; set; }
    }
}
