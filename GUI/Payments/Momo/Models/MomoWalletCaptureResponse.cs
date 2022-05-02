using GUI.Payments.Momo.Exceptions;

namespace GUI.Payments.Momo.Models
{
    public class MomoWalletCaptureResponse : MomoWalletBaseObject
    {
        public long ResponseTime { get; set; }

        public string Message { get; set; }

        public int ResultCode { get; set; }

        public string PayUrl { get; set; }

        public MomoWalletException[] SubErrors { get; set; }

        public override string GetSecurityMessage()
        {
            throw new System.NotImplementedException();
        }
    }
}
