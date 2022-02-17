using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace AuthServer.Identities
{
    public class ApplicationRoleStore : RoleStore<Role, ApplicationDbContext, Guid>
    {
        public ApplicationRoleStore(ApplicationDbContext context, IdentityErrorDescriber? describer = null) 
            : base(context, describer)
        {
        }

        protected override IdentityRoleClaim<Guid> CreateRoleClaim(Role role, Claim claim)
        {
            return new IdentityRoleClaim<Guid>
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };
        }
    }
}
