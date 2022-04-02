using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace AuthServer.Abstractions
{
    public class AuthorizeWithoutRedirectToSignIn : ActionFilterAttribute
    {
        private readonly string _role = string.Empty;

        private readonly bool _requireSpecifiedRole = false;

        public AuthorizeWithoutRedirectToSignIn() { }

        public AuthorizeWithoutRedirectToSignIn(string role)
        {
            _requireSpecifiedRole = true;
            _role = role;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext!.User!.Identity!.IsAuthenticated)
            {
                var statusCode = _requireSpecifiedRole && !context.HttpContext.User.IsInRole(_role) 
                    ? StatusCodes.Status403Forbidden
                    : StatusCodes.Status401Unauthorized;
                context.Result = new StatusCodeResult(statusCode);
            }
            else
            {
                await next();
            }
        }
    }
}
