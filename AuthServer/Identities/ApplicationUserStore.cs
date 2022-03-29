using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace AuthServer.Identities
{
    public class ApplicationUserStore : UserStore<User, Role, ApplicationDbContext, Guid, IdentityUserClaim<Guid>,
        UserRole, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>
    {
        public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber? describer = null)
            : base(context, describer)
        {
        }

        protected override IdentityUserClaim<Guid> CreateUserClaim(User user, Claim claim)
        {
            var userClaim = new IdentityUserClaim<Guid> { UserId = user.Id };
            userClaim.InitializeFromClaim(claim);
            return userClaim;
        }

        protected override IdentityUserLogin<Guid> CreateUserLogin(User user, UserLoginInfo login)
        {
            return new IdentityUserLogin<Guid>
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                ProviderDisplayName = login.ProviderDisplayName,
                LoginProvider = login.LoginProvider
            };
        }

        protected override UserRole CreateUserRole(User user, Role role)
        {
            return new UserRole
            {
                RoleId = role.Id,
                UserId = user.Id
            };
        }

        protected override IdentityUserToken<Guid> CreateUserToken(User user, string loginProvider, string name, string value)
        {
            return new IdentityUserToken<Guid>
            {
                UserId = user.Id,
                LoginProvider = loginProvider,
                Name = name,
                Value = value
            };
        }
    }
}
