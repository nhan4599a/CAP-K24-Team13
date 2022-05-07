using Shared.Models;
using System;

namespace Shared.DTOs
{
    public class InvoiceStatusChangedHistoryDTO
    {
        public DateTime ChangedTime { get; set; }

        public InvoiceStatus Status { get; set; }
    }
}
