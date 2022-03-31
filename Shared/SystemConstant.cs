using System.Security.Claims;

namespace Shared
{
    public sealed class SystemConstant
    {
        public static class Roles
        {
            public const string ADMIN_TEAM_5 = "AdminTeam5";

            public const string ADMIN_TEAM_13 = "AdminTeam13";

            public const string SHOP_OWNER = "ShopOwner";

            public const string CUSTOMER = "Customer";
        }

        public static class Identity
        {
            public const string SHOP_ID_CLAIM_TYPE = "ShopId";

            public const string USER_ID_CLAIM_TYPE = ClaimTypes.NameIdentifier;
        }

        public static class Authentication
        {
            public const string ACCESS_TOKEN_KEY = "access_token";
        }

        public static class Common
        {
            public const string CART_ITEMS_KEY = "CartItems";
        }
    }
}
