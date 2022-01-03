namespace AuthServer.Models
{
    public class SignInModel : AuthenticationModelBase
    {
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
