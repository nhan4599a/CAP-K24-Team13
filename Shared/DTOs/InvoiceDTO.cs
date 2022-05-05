using Shared.Models;
using System;

namespace Shared.DTOs
{
    public class InvoiceDTO
    {
        public int InvoiceId { get; set; }

        public string InvoiceCode { get; set; }

        public string RefId { get; set; }

        public string ReceiverName { get; set; }

        public string PhoneNumber { get; set; }

        public string ShippingAddress { get; set; }

        public DateTime CreatedAt { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public InvoiceStatus Status { get; set; }

        public int ShopId { get; set; }
    }
}