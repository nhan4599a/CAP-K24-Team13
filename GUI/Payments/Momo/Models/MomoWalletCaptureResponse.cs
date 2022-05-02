using GUI.Payments.Abstraction;
using GUI.Payments.Momo.Exceptions;

namespace GUI.Payments.Momo.Models
{
    public class MomoWalletCaptureResponse : PaymentResponse
    {
        public string PartnerCode { get; set; }

        public string OrderId { get; set; }

        public string RequestId { get; set; }

        public int Amount { get; set; }

        public long ResponseTime { get; set; }

        public string Message { get; set; }

        public int ResultCode { get; set; }

        public string PayUrl { get; set; }

        public MomoWalletException[] SubErrors { get; set; }

        public override bool IsErrorResponse()
        {
            return ResultCode != 0;
        }
    }
}
