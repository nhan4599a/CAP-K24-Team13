using Shared;
using System;
using System.Security.Claims;

namespace GUI.Extensions
{
    public static class UserExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            return Guid.Parse(principal.FindFirstValue(SystemConstant.Identity.USER_ID_CLAIM_TYPE));
        }

        public static string GetUsername(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            return principal.Identity.Name;
        }

        public static int? GetShopId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var stringValue = principal.FindFirstValue(SystemConstant.Identity.SHOP_ID_CLAIM_TYPE);
            return stringValue == null ? null : int.Parse(stringValue);
        }
    }
}
