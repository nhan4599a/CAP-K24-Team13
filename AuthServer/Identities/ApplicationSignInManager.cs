using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AuthServer.Identities
{
    public class ApplicationSignInManager : SignInManager<User>
    {
        public ApplicationSignInManager(UserManager<User> userManager, IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<User> principalFactory, IOptions<IdentityOptions> options, 
            ILogger<SignInManager<User>> logger, IAuthenticationSchemeProvider schemeProvider,
            IUserConfirmation<User> userConfirmation)
            : base(userManager, contextAccessor, principalFactory,options, logger, schemeProvider, userConfirmation)
        { }
    }
}
