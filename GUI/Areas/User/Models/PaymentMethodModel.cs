namespace GUI.Areas.User.Models
{
    public class PaymentMethodModel
    {
        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string[] InvoiceCode { get; set; }

        public CheckOutModel[] Products { get; set; }
    }
}
