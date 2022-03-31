using AuthServer.Identities;
using DatabaseAccessor.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthServer.Factories
{
    public class ApplicationUserClaimsPrincipleFactory : UserClaimsPrincipalFactory<User>
    {
        private readonly ApplicationUserManager _userManager;

        public ApplicationUserClaimsPrincipleFactory(ApplicationUserManager userManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var role = (await _userManager.GetRolesAsync(user))[0];
            identity.AddClaim(new Claim(JwtClaimTypes.Role, role));
            if (role == SystemConstant.Roles.SHOP_OWNER)
                identity.AddClaim(new Claim("ShopId", user.ShopId!.Value.ToString()));
            return identity;
        }
    }
}
