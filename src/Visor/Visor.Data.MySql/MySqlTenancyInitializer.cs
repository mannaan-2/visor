using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shigar.Core.Tenants.Services;
using System.Collections.Generic;
using System.Linq;
using Visor.Data.MySql.Abstractions;
using Visor.Data.MySql.Identity;
using Visor.Data.MySql.Identity.Entities;
using Visor.Data.MySql.Tenancy.Pipelines;

namespace Visor.Data.MySql
{
    public static class MySqlTenancyInitializer
    {
        public static TenantResolutionBuilder InitTenantResolution(
            this IApplicationBuilder builder)
        {
            builder.UseMiddleware<InitializeTenantResolutionProcessor>();
            return new TenantResolutionBuilder(builder);
        }

        public static void AddTenants(this IServiceCollection services)
        {
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddTransient<ITenantContext, TenantContext>();
        }
        public static void InitializeDefaultTenant(this IServiceCollection services, string connectionString)
        {
            //  DesignTimeDbContextFactory.SetConnectionString(connectionString);

            var serviceProvider = services.BuildServiceProvider();
            var tenantRepository = serviceProvider.GetService<ITenantRepository>();
            var defaultTenant = AddTenantToDb("local", "Default tenant", "For dev", "localhost", tenantRepository);

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
        private static ITenant AddTenantToDb(string key, string name, string desciption, string host, ITenantRepository tenantRepository)
        {
            var existing = tenantRepository.FindByKey(key);
            if (existing != null)
                return existing;
            var tenant = new Tenant
            {
                Active = true,
                TenantId = -1,
                Name = name,
                Key = key,
                Host = host,
                Description = desciption
            };
            return tenantRepository.CreateOrUpdate(tenant);
        }
    }
}
