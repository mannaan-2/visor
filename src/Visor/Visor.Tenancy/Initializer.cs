using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Visor.Data.MySql.Abstractions;
using Visor.Tenancy.Abstractions;
using Visor.Tenancy.Pipelines;
using Visor.Tenancy.Tenancy.Pipelines;

namespace Visor.Tenancy
{
    public static class Initializer
    {
        public static void AddTenancy(this IServiceCollection services)
        {
           services.AddTransient<ITenantContext, TenantContext>();
        }
      
        public static IApplicationBuilder UseTenants(
           this IApplicationBuilder app)
        {
            app.InitTenantResolution()
             .Then<AttemptResolutionByHost>()
             .Then<AttemptResolutionByQueryString>()
             //.Then<AttemptResolutionByCookie>()
             .Then<VerifyTenantResolution, TenantVerificationOptions>(new TenantVerificationOptions(new List<string>()
             {
                   "/.well-known/openid-configuration",
                   "/.well-known/openid-configuration/jwks"
             }));
            return app;
        }
        private static TenantResolutionBuilder InitTenantResolution(
         this IApplicationBuilder builder)
        {
            builder.UseMiddleware<InitializeTenantResolutionProcessor>();
            return new TenantResolutionBuilder(builder);
        }
    }

}
