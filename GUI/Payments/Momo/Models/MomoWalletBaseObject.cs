namespace GUI.Payments.Momo.Models
{
    public abstract class MomoWalletBaseObject
    {
        public string PartnerCode { get; set; }

        public string OrderId { get; set; }

        public string RequestId { get; set; }

        public int Amount { get; set; }

        public abstract string GetSecurityMessage();
    }
}
