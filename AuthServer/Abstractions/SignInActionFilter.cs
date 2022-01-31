using AuthServer.Identities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthServer.Abstractions
{
    public class SignInActionFilter : IAsyncActionFilter
    {
        private readonly ApplicationSignInManager _signInManager;

        public SignInActionFilter(ApplicationSignInManager signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
            var controller = context.Controller as Controller;
            var action = context.RouteData.Values["action"]!.ToString();
            if (action == "SignIn")
            {
                var externalProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
                controller!.ViewData["ExternalProviders"] = externalProviders;
            }
        }
    }
}
