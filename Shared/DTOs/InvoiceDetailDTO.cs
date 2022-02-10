using System;

namespace Shared.DTOs
{
    public class InvoiceDetailDTO
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}