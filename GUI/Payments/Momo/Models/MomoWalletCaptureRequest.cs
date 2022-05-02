namespace GUI.Payments.Momo.Models
{
    public class MomoWalletCaptureRequest : MomoWalletBaseObject
    {
        public string AccessKey { get; set; }

        public string RequestType { get; } = "captureWallet";

        public string IpnUrl { get; set; }

        public string RedirectUrl { get; set; }

        public string OrderInfo { get; set; }

        public string ResponseLanguage { get; set; }

        public string ExtraData { get; set; }

        public string Signature { get; set; }

        public override string GetSecurityMessage()
        {
            return $"accessKey={AccessKey}&amount={Amount}&extraData={ExtraData}&ipnUrl={IpnUrl}&orderId={OrderId}&orderInfo={OrderInfo}&partnerCode={PartnerCode}&redirectUrl={RedirectUrl}&requestId={RequestId}&requestType={RequestType}";
        }
    }
}
