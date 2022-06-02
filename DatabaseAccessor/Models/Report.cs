using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("Reports", Schema = "dbo")]
    public class Report
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid ReporterId { get; set; }

        public Guid AffectedUserId { get; set; }

        public int AffectedInvoiceId { get; set; }

        public virtual User Reporter { get; set; }

        public virtual User AffectedUser { get; set; }

        public virtual Invoice AffectedInvoice { get; set; }
    }
}
