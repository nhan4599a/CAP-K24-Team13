using Shared.Models;
using System;

namespace Shared.DTOs
{
    public class InvoiceDTO
    {
        public string InvoiceCode { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public InvoiceStatus Status { get; set; }

        public double TotalPrice { get; set; }

        public bool IsReported { get; set; }
    }
}

