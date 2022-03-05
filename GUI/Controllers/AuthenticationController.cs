﻿using GUI.Extensions;
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
                return ApiResult.CreateErrorResult(401, "User is not logged in");
            return ApiResult<string>.CreateSucceedResult(await HttpContext.GetTokenAsync("access_token"));
        }

        [ActionName("id")]
        public ApiResult GetCurrentUserId()
        {
            if (!User.Identity.IsAuthenticated)
                return ApiResult.CreateErrorResult(401, "User is not logged in");
            return ApiResult<string>.CreateSucceedResult(User.GetUserId().ToString());
        }

        [ActionName("shop")]
        public ApiResult GetCurrentShopId()
        {
            if (!User.Identity.IsAuthenticated)
                return ApiResult.CreateErrorResult(401, "User is not logged in");
            if (!User.IsInRole(Roles.SHOP_OWNER))
                return ApiResult.CreateErrorResult(403, "User is not shop owner");
            return ApiResult<int>.CreateSucceedResult(User.GetShopId().Value);
        }

        public SignOutResult SignOut(string redirectUrl = "/")
        {
            var authenticationSchemes = new[]
            {
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme
            };
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };
            return new SignOutResult(authenticationSchemes, authenticationProperties);
        }

        public ChallengeResult SignIn(string redirectUrl = "/")
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            }, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
