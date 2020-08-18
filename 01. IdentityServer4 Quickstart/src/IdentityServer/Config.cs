// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("resourceapi", "Resource API")
                {
                    Scopes = {new Scope("api.read")}
                }
            };
        }



        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client {
                ClientId = "angular_spa",
                ClientName = "Angular 4 Client",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                AllowedScopes = new List<string> {"openid", "profile", "api.read"},
                RedirectUris = new List<string> {"https://localhost:4200/auth-callback", "https://localhost:4200/silent-refresh.html"},
                PostLogoutRedirectUris = new List<string> {"https://localhost:4200/"},
                AllowedCorsOrigins = new List<string> {"https://localhost:4200"},
                AllowAccessTokensViaBrowser = true

                }
            };
        }

    }
}