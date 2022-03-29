using AuthServer.Identities;
using DatabaseAccessor.Models;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Services
{
    public class ApplicationUserProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<User> _claimsPrincipleFactory;

        private readonly ApplicationUserManager _userManager;

        public ApplicationUserProfileService(IUserClaimsPrincipalFactory<User> claimsPrincipalFactory,
            ApplicationUserManager userManager)
        {
            _claimsPrincipleFactory = claimsPrincipalFactory;
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
                throw new ArgumentException("Something went wrong!");
            var principle = await _claimsPrincipleFactory.CreateAsync(user);
            context.IssuedClaims = principle.Claims.ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
                throw new ArgumentException("Something went wrong!");
            var result = user != null
                && user.Status == AccountStatus.Available;
            context.IsActive = result;
        }
    }
}
