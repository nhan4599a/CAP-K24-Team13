using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor.Models
{
    [Table("AspNetRoles", Schema = "dbo")]
    public class Role : IdentityRole<Guid>
    {
    }
}
