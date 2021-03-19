using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using Visor.Data.MySql.Abstractions;
using Visor.Tenancy.Pipelines;
using Visor.Tenancy.Tenancy.Pipelines;

namespace Visor.Tenancy
{
    public static class Initializer
    {
        public static TenantResolutionBuilder InitTenantResolution(
           this IApplicationBuilder builder)
        {
            builder.UseMiddleware<InitializeTenantResolutionProcessor>();
            return new TenantResolutionBuilder(builder);
        }
        public static IApplicationBuilder UseTenants(
           this IApplicationBuilder app)
        {
            app.InitTenantResolution()
             .Then<AttemptResolutionByHost>()
             .Then<AttemptResolutionByQueryString>()
             .Then<AttemptResolutionByReferrer>()
             .Then<AttemptResolutionByCookie>()
             .Then<VerifyTenantResolution, TenantVerificationOptions>(new TenantVerificationOptions(new List<string>()
             {
                   "/.well-known/openid-configuration",
                   "/.well-known/openid-configuration/jwks"
             }));
            return app;
        }
    }

}
