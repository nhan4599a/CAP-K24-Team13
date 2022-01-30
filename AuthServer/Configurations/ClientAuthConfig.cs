using IdentityServer4;
using IdentityServer4.Models;

namespace AuthServer.Configurations
{
    public class ClientAuthConfig
    {
        public static IEnumerable<Client> Clients => new[]
        {
            new Client
            {
                ClientId = "products-client",
                ClientName = "products-client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = new[]
                {
                    new Secret("CapK24Team13".Sha256())
                },
                AllowedScopes = new[]
                {
                    "product.read", "product.write"
                }
            },
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
                    "https://localhost:44349/signin-oidc"
                },
                AllowedScopes = new[]
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "product.read", "product.write"
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
            new IdentityResource("product", new[]
            {
                "role"
            })
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
                    "product.read", "product.write"
                },
                ApiSecrets = new[]
                {
                    new Secret("CapK24Team13".Sha256())
                }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new[]
        {
            new ApiScope
            {
                Name = "product.read",
                DisplayName = "product.read",
                Description = "Allow application to have read permission on product"
            },
            new ApiScope
            {
                Name = "product.write",
                DisplayName = "product.write",
                Description = "Allow application to have write permission on product"
            }
        };
    }
}
