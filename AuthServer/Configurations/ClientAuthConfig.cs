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
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources => new[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource
            {
                Name = "Role",
                UserClaims = new[]
                {
                    "Role"
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
                    "product.read", "product.write"
                },
                ApiSecrets = new[]
                {
                    new Secret("CapK24Team13".Sha256())
                },
                UserClaims = new[]
                {
                    "Role"
                }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new[]
        {
            new ApiScope("product.read", "Read access to product API"),
            new ApiScope("product.write", "Write access to product API")
        };
    }
}
