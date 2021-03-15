using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visor.Data.MySql.Constants
{

    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()

            };

        public static IEnumerable<ApiResource> Apis =>
           new List<ApiResource>
        {
             new ApiResource("users", "User management api"){ 
                 Scopes = new List<string> { "anon-user", "authenticated-user" }
             },
             new ApiResource("api", "dummy"),
             new ApiResource("forms", "Forms api"),

        };
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
    {
        new ApiScope(name: "anon-user",   displayName: "Anonymous users"),
        new ApiScope(name: "authenticated-user",  displayName: "Authenticated users"),
    };
        }
        public static IEnumerable<Client> swaggerClient => new List<Client>
        {
            new Client
            {
                ClientId = "swagger",
                ClientName = "Swagger UI",
                ClientSecrets = { new Secret("secret".Sha256()) }, // change me!

                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                RequirePkce = true,
                RequireClientSecret = true,

                RedirectUris = { "https://localhost:44382/swagger/oauth2-redirect.html" },
                AllowedCorsOrigins = { "https://localhost:44382" },
                AllowedScopes = { "anon-user", "authenticated-user" }
            }
        };
    }
}
