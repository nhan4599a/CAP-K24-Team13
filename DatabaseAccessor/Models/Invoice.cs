using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("Invoices")]
    public class Invoice
    {
        public int Id { get; set; }

        public string InvoiceCode { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public string ShippingAddress { get; set; }
        
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Phone { get; set; }

        public string Note { get; set; }

        public InvoiceStatus Status { get; set; }

        public int ShopId { get; set; }

        public virtual User User { get; set; }

        public virtual IList<InvoiceDetail> Details { get; set; } = new List<InvoiceDetail>();

        public virtual IList<InvoiceStatusChangedHistory> StatusChangedHistory { get; set; }
    }
}
