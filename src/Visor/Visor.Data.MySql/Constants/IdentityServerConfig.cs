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
             new ApiResource("users", "User management api"),
             new ApiResource("api", "dummy"),
             new ApiResource("forms", "Forms api"),

        };

    }
}
