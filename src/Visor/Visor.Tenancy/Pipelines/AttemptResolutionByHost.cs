using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Visor.Data.MySql.Abstractions;
using Visor.Tenancy.Abstractions;

namespace Visor.Tenancy.Pipelines
{
    public class AttemptResolutionByHost
    {
        private readonly RequestDelegate _next;
        public AttemptResolutionByHost(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantRepository tenantRepository, ITenantContext tenantContext)
        {
            if (tenantContext.Resolved)
                await _next(context);
            var host = context.Request.Host.Host;
            var tenant = tenantRepository.FindByHostName(host);
            if (tenant != null && tenant.Active)
            {
                tenantContext.Set(tenant.Key);
            }
            await _next(context);
        }
    }
}
