using Microsoft.Extensions.DependencyInjection;
using Shigar.Core.Tenants.Services;
using Visor.Data.MySql.Identity.Entities;
using Visor.Tenancy;
using Visor.Tenancy.Abstractions;

namespace Visor.Data.MySql
{
    public static class MySqlTenancyInitializer
    {

        public static void AddMySqlTenancy(this IServiceCollection services)
        {
            services.AddTenancy();
            services.AddTransient<ITenantRepository, TenantRepository>();
            
        }
        public static void InitializeDefaultTenant(this IServiceCollection services, string connectionString)
        {
            //  DesignTimeDbContextFactory.SetConnectionString(connectionString);

            var serviceProvider = services.BuildServiceProvider();
            var tenantRepository = serviceProvider.GetService<ITenantRepository>();
            var defaultTenant = AddTenantToDb("local", "Default tenant", "For dev", "localhost", tenantRepository);
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
