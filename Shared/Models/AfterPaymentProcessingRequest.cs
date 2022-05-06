namespace Shared.Models
{
    public class AfterPaymentProcessingRequest
    {
        public string AccessToken { get; set; }

        public string WalletIpnRequest { get; set; }
    }
}
