using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("InvoiceStatusChangedHistories")]
    public class InvoiceStatusChangedHistory
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }

        public InvoiceStatus OldStatus { get; set; }

        public InvoiceStatus NewStatus { get; set; }

        public DateTime ChangedDate { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}
