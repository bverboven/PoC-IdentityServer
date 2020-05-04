// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

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
            new[]
            {
                new ApiResource("api1", "My API #1")
            };
        
        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = "accountManager",
                    ClientName = "Account management",
                    ClientSecrets = { new Secret("Account_Management".Sha256()) },

                    //AccessTokenType = AccessTokenType.Reference,
                    RequirePkce = true,
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                    
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    AllowedScopes = { "openid", "profile", "roles", "permissions" },

                    RedirectUris = { "https://localhost:5001/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5001/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" }
                },
            };
    }
}