﻿using IdentityModel;
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
                    "roles", "shop"
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
    }
}
