using Microsoft.AspNetCore.Identity;
using System;

namespace DatabaseAccessor.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateOnly? DoB { get; set; }
 
        public AccountStatus Status { get; set; }
    }
}
