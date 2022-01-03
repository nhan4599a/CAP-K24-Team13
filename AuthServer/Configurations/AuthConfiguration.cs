using IdentityServer4.Models;

namespace AuthServer.Configurations
{
    public class AuthConfiguration
    {
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource
            {
                Name = "Product.Api",
                Description = "Product Api"
            }
        };

        public static IEnumerable<Client> Clients => new Client[]
        {
            new Client
            {
                ClientId = "mvc-client",
                ClientName = "MVC client",
                RequireConsent = false,
                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                ClientSecrets = { new Secret("ef85af7b3de933879fe6c9931c892e9d".Sha256()) },
                RedirectUris = { "https://localhost:44349/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44349/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44349/signout-callback-oidc" },
                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "Product.Api" }
            },
            new Client
            {
                ClientId = "admin-client",
                ClientName = "Admin client",
                ClientUri = "https://localhost:44349",
                RequireConsent = false,
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                AllowAccessTokensViaBrowser = true,
                RedirectUris =
                {
                    "https://localhost:44349/admin"
                },
                PostLogoutRedirectUris = { "https://localhost:44349/" },
                AllowedCorsOrigins = { "https://localhost:44349" },
                AllowedScopes = { "openid", "profile", "Product.Api" }
            }
        };
    }
}
