using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthServer.Configurations
{
    public class ClientAuthConfig
    {
        public static IEnumerable<Client> Clients => new[]
        {
            new Client
            {
                ClientId = "oidc-client",
                ClientName = "OIDC client",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = new[]
                {
                    new Secret("CapK24Team13".Sha256())
                },
                RedirectUris = new[]
                {
                    "https://cap-k24-team13.herokuapp.com/signin-oidc"
                },
                PostLogoutRedirectUris = new[]
                {
                    "https://cap-k24-team13.herokuapp.com/signout-callback-oidc"
                },
                AllowedScopes = new[]
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "product.api", "interface.api", "order.api", "checkout.api", "rating.api", "roles", "shop"
                },
                RequirePkce = true,
                AllowPlainTextPkce = false,
                AllowOfflineAccess = true
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource
            {
                Name = "roles",
                DisplayName = "Roles",
                UserClaims =
                {
                    JwtClaimTypes.Role
                }
            },
            new IdentityResource
            {
                Name = "shop",
                DisplayName = "Shop",
                UserClaims =
                {
                    "ShopId"
                }
            }
        };

        public static IEnumerable<ApiResource> ApiResources => new[]
        {
            new ApiResource
            {
                Name = "product",
                DisplayName = "Product API",
                Description = "Allow the application to access product API",
                Scopes = new[]
                {
                    "product.api"
                },
                ApiSecrets = new[]
                {
                    new Secret("CapK24Team13".Sha256())
                },
                UserClaims =
                {
                    JwtClaimTypes.Role, "ShopId"
                }
            },
            new ApiResource
            {
                Name = "interface",
                DisplayName = "Interface API",
                Description = "Allow the application to access interface API",
                Scopes = new[]
                {
                    "interface.api"
                },
                ApiSecrets = new[]
                {
                    new Secret("CapK24Team13".Sha256())
                },
                UserClaims =
                {
                    JwtClaimTypes.Role, "ShopId"
                }
            },
            new ApiResource
            {
                Name = "order",
                DisplayName = "Order API",
                Description = "Allow the application to access order API",
                Scopes = new[]
                {
                    "order.api"
                },
                ApiSecrets = new[]
                {
                    new Secret("CapK24Team13".Sha256())
                },
                UserClaims =
                {
                    JwtClaimTypes.Role, "ShopId"
                }
            },
            new ApiResource
            {
                Name = "checkout",
                DisplayName = "Checkout API",
                Description = "Allow the application to access checkout API",
                Scopes = new[]
                {
                    "checkout.api"
                },
                ApiSecrets = new[]
                {
                    new Secret("CapK24Team13".Sha256())
                },
                UserClaims =
                {
                    JwtClaimTypes.Role, "ShopId"
                }
            },
            new ApiResource
            {
                Name = "rating",
                DisplayName = "Rating API",
                Description = "Allow the application to access rating API",
                Scopes = new[]
                {
                    "rating.api"
                },
                ApiSecrets = new[]
                {
                    new Secret("CapK24Team13".Sha256())
                },
                UserClaims =
                {
                    JwtClaimTypes.Role, "ShopId"
                }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new[]
        {
            new ApiScope
            {
                Name = "product.api",
                DisplayName = "product.api",
                Description = "Allow application to have access permission on product API"
            },
            new ApiScope
            {
                Name = "interface.api",
                DisplayName = "interface.api",
                Description = "Allow application to have write permission on interface API"
            },
            new ApiScope
            {
                Name = "order.api",
                DisplayName = "order.api",
                Description = "Allow application to have write permission on order API"
            },
            new ApiScope
            {
                Name = "checkout.api",
                DisplayName = "checkout.api",
                Description = "Allow application to have write permission on checkout API"
            },
            new ApiScope
            {
                Name = "rating.api",
                DisplayName = "rating.api",
                Description = "Allow application to have write permission on rating API"
            }
        };
    }
}
