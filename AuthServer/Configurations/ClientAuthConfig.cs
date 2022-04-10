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
                    "roles", "shop", IdentityServerConstants.LocalApi.ScopeName
                },
                RequirePkce = true,
                AllowPlainTextPkce = false,
                AllowOfflineAccess = true,
                UserSsoLifetime = 43200,
                AccessTokenLifetime = 43200,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Absolute
            },
            new Client
            {
                ClientId = "oidc-client-team5",
                ClientName = "OIDC client Team5",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = new[]
                {
                    new Secret("CapK24Team5".Sha256())
                },
                RedirectUris = new[]
                {
                    "https://emallsolution-admin.herokuapp.com/signin-oidc",
                    "https://localhost:44382/signin-oidc"
                },
                PostLogoutRedirectUris = new[]
                {
                    "https://emallsolution-admin.herokuapp.com/signout-callback-oidc",
                    "https://localhost:44382/signout-callback-oidc"
                },
                AllowedScopes = new[]
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "roles", "shop", IdentityServerConstants.LocalApi.ScopeName
                },
                RequirePkce = true,
                AllowPlainTextPkce = false,
                AllowOfflineAccess = true,
                UserSsoLifetime = 43200,
                AccessTokenLifetime = 43200,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Absolute
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

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };
    }
}
