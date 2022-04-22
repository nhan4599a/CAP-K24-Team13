using Shared.Models;
using System;

namespace Shared.DTOs
{
    public class OrderItemDTO
    {
        public int InvoiceId { get; set; }

        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }   

        public DateTime CreatedAt { get; set; }

        public double Price { get; set; }   

        public string Image { get; set; }

        public InvoiceStatus Status { get; set; }

        public bool CanBeRating { get; set; }
    }
}
