using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class MomoWalletIpnRequest : PaymentRequest
    {
        public string AccessKey { get; set; }

        public string PartnerCode { get; set; }

        public string OrderId { get; set; }

        public string RequestId { get; set; }

        public int Amount { get; set; }

        public string OrderInfo { get; set; }

        public string OrderType { get; set; }

        [JsonPropertyName("transId")]
        public string TransactionId { get; set; }

        public int ResultCode { get; set; }

        public string Message { get; set; }

        public string PayType { get; set; }

        public long ResponseTime { get; set; }

        public string ExtraData { get; set; }

        public string Signature { get; set; }

        public override string GetSecurityMessage()
        {
            return $"accessKey={AccessKey}&amount={Amount}&extraData={ExtraData}&message={Message}&orderId={OrderId}&orderInfo={OrderInfo}&orderType={OrderType}&partnerCode={PartnerCode}&payType={PayType}&requestId={RequestId}&responseTime={ResponseTime}&resultCode={ResultCode}&transId={TransactionId}";
        }
    }
}
