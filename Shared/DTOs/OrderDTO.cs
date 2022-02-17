using Shared.Models;

namespace Shared.DTOs
{
    public class OrderDTO
    {
        public string OrderCode { get; set; }

        public string ShippingAddress { get; set; }

        public string CustomerName { get; set; }

        public string Phone { get; set; }

        public string CreatedAt { get; set; }

        public InvoiceStatus Status { get; set; }
    }
}
