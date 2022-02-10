using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class InvoiceStatusChangedHistoryDTO
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public InvoiceStatus OldStatus { get; set; }

        public InvoiceStatus NewStatus { get; set; }

        public DateTime ChangedDate { get; set; }

    }
}
