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
            return Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public static string GetUsername(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            return principal.Identity.Name;
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            return principal.FindFirstValue(ClaimTypes.Email);
        }

        public static int? GetShopId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            var stringValue = principal.FindFirstValue("ShopId");
            return stringValue == null ? null : int.Parse(stringValue);
        }
    }
}
