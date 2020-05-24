// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;

namespace Identity.Sts
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
            new[] {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles", "Roles", new[] {"role"}),
                new IdentityResource("permissions", "Permissions", new[] {"permission"}),
            };

        public static IEnumerable<ApiResource> Apis =>
            new[] {
                new ApiResource("accountApi", "Account Api") {
                    ApiSecrets = {new Secret("Account_Api".Sha256())},
                    UserClaims = {"role", "permission", JwtClaimTypes.Name, JwtClaimTypes.FamilyName, JwtClaimTypes.GivenName,}
                }
            };

        public static IEnumerable<Client> Clients =>
            new[] {
                new Client {
                    ClientId = "accountManager",
                    ClientName = "Account management",
                    ClientSecrets = {new Secret("Account_Management".Sha256())},

                    AccessTokenType = AccessTokenType.Reference,
                    RequirePkce = true,
                    AllowOfflineAccess = true,
                    RequireClientSecret = true,
                    RequireConsent = false,

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {"openid", "profile", "roles", "permissions"},
                    
                    // required
                    RedirectUris = {"https://localhost:5001/signin-oidc"},
                    FrontChannelLogoutUri = "https://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = {"https://localhost:5001/signout-callback-oidc"}
                },
                new Client {
                    ClientId = "consoleApp",
                    ClientName = "Console app",
                    ClientSecrets = {new Secret("Console_App".Sha256())},

                    RequireClientSecret = true,
                    AllowedGrantTypes = {GrantType.ClientCredentials, GrantType.ResourceOwnerPassword},
                    AllowedScopes = {"openid", "profile", "accountApi"}
                },
                new Client {
                    ClientId = "dummyClient",
                    ClientName = "Dummy Client",
                    ClientSecrets = {new Secret("Dummy_Client".Sha256())},

                    AccessTokenType = AccessTokenType.Jwt,
                    RequirePkce = true,
                    AllowOfflineAccess = true,
                    RequireClientSecret = true,
                    RequireConsent = false,

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {"openid", "profile", "roles", "permissions"},

                    // required
                    RedirectUris = {"https://localhost:5003/signin-oidc"},
                    FrontChannelLogoutUri = "https://localhost:5003/signout-oidc",
                    PostLogoutRedirectUris = {"https://localhost:5003/signout-callback-oidc"}
                }
            };
    }
}