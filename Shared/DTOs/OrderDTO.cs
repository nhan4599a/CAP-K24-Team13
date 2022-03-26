using Shared.Models;
using System;

namespace Shared.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public string InvoiceCode { get; set; }

        public string ShippingAddress { get; set; }

        public string CustomerName { get; set; }

        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; }

        public InvoiceStatus Status { get; set; }
    }
}
