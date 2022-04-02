using Microsoft.AspNetCore.Identity;
using System;

namespace DatabaseAccessor.Models
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public virtual Role Role { get; set; }

        public virtual User User { get; set; }
    }
}
