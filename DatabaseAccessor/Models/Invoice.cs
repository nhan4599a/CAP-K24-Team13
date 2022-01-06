using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("Invoices")]
    public class Invoice
    {
        public int Id { get; set; }

        public string InvoiceCode { get; set; }

        public Guid UserId { get; set; }

        public DateTime Created { get; set; }

        public string ShippingAddress { get; set; }

        public InvoiceStatus Status { get; set; }

        public virtual IList<InvoiceDetail> Details { get; set; }
    }
}
