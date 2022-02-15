using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace DatabaseAccessor.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateOnly? DoB { get; set; }

        public AccountStatus Status { get; set; }

        public virtual IList<Invoice> Invoices { get; set; }

        public virtual Cart Cart { get; set; }
    }
}
