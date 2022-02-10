using Shared.Models;
using System;
using System.Collections.Generic;

namespace Shared.DTOs
{
    public class InvoiceDTO
    {
        public int Id { get; set; }

        public string InvoiceCode { get; set; }

        public Guid UserId { get; set; }

        public DateTime Created { get; set; }

        public string ShippingAddress { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Note { get; set; }

        public InvoiceStatus Status { get; set; }

        public int ShopId { get; set; }

        public IList<InvoiceDetailDTO>? Details { get; set; } = new List<InvoiceDetailDTO>();

        public IList<InvoiceStatusChangedHistoryDTO>? StatusChangedHistory { get; set; }
    }
}
