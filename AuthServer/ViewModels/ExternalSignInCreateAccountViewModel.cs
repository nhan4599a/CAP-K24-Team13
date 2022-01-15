namespace AuthServer.ViewModels
{
    public class ExternalSignInCreateAccountViewModel
    {
        public string Provider { get; }

        public string ProviderId { get; }

        public string? Email { get; set; }

        public string ReturnUrl { get; set; } = "~/";

        public string? SessionId { get; set; }

        public string? IdToken { get; set; }

        public ExternalSignInCreateAccountViewModel(string provider, string providerId)
        {
            Provider = provider;
            ProviderId = providerId;
        }
    }
}
