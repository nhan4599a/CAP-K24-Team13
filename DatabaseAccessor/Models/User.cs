using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("AspNetUsers", Schema = "dbo")]
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateOnly? DoB { get; set; }

        public AccountStatus Status { get; set; }

        public int? ShopId { get; set; }

        public virtual IList<Invoice> Invoices { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
