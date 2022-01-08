using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;

namespace AuthServer.Controllers
{
    public class AuthorizationController : Controller
    {
        [HttpPost("/connect/token")]
        public async Task<IActionResult> ExchangeToken()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("Something went wrong!");

            ClaimsPrincipal? principle;
            if (request.IsClientCredentialsGrantType())
            {
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                identity.AddClaim(OpenIddictConstants.Claims.Subject, 
                    request.ClientId ?? throw new InvalidOperationException());

                principle = new ClaimsPrincipal(identity);
                principle.SetScopes(request.GetScopes());
            }
            else if (request.IsAuthorizationCodeGrantType())
            {
                principle = 
                    (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme))
                        .Principal;
            }
            else
            {
                throw new NotSupportedException($"grant type \"{request.GrantType}\" is not supported.");
            }
            return SignIn(principle!, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        [HttpGet("/connect/authorize")]
        [HttpPost("/connect/authorize")]
        public async Task<IActionResult> Authorize()
        {
            var request = HttpContext.GetOpenIddictServerRequest() ??
                throw new InvalidOperationException("Something went wrong!");

            var signInResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!signInResult.Succeeded)
            {
                return Challenge(
                    authenticationSchemes: CookieAuthenticationDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties
                    {
                        RedirectUri = Request.PathBase + Request.Path + QueryString.Create(
                            Request.HasFormContentType ? Request.Form.ToList() : Request.Query.ToList())
                    });
            }

            var claims = new List<Claim>
            {
                new Claim(OpenIddictConstants.Claims.Subject, signInResult.Principal.Identity?.Name!)
            };

            var claimsIdentity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);

            claimsPrinciple.SetScopes(request.GetScopes());

            return SignIn(claimsPrinciple, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }
}
