using GUI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI.Controllers
{
    public class AuthenticationController : Controller
    {
        [ActionName("token")]
        public async Task<ApiResult> GetCurrentUserAccessToken()
        {
            if (!User.Identity.IsAuthenticated)
                return ApiResult.CreateErrorResult(403, "User is not logged in");
            return ApiResult<string>.CreateSucceedResult(await HttpContext.GetTokenAsync("access_token"));
        }

        [ActionName("id")]
        public ApiResult GetCurrentUserId()
        {
            if (!User.Identity.IsAuthenticated)
                return ApiResult.CreateErrorResult(403, "User is not logged in");
            return ApiResult<string>.CreateSucceedResult(User.GetUserId().ToString());
        }

        public new IActionResult SignOut()
        {
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
