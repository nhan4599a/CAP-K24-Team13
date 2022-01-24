using AuthServer.Identities;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Security.Claims;

namespace AuthServer.Services
{
    public class UserProfileService : IProfileService
    {
        private readonly ApplicationUserManager _userManager;

        public UserProfileService(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.FindByNameAsync(context.Subject!.Identity!.Name);
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Email, user.Email)
            };
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.FindByNameAsync(context.Subject!.Identity!.Name);
            context.IsActive = user != null;
        }
    }
}
