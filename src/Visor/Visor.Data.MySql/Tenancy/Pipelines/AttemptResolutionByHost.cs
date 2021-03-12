using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Visor.Data.MySql.Abstractions;

namespace Visor.Data.MySql.Tenancy.Pipelines
{
    public class AttemptResolutionByHost : IMiddleware
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantContext _tenantContext;

        public AttemptResolutionByHost(ITenantRepository tenantRepository, ITenantContext tenantContext)
        {
            _tenantRepository = tenantRepository;
            _tenantContext = tenantContext;

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_tenantContext.Resolved)
                await next(context);
            var host = context.Request.Host.Host;
            var tenant = _tenantRepository.FindByHostName(host);
            if (tenant != null && tenant.Active)
            {
                _tenantContext.Set(tenant.Key);
            }
            await next(context);
        }
    }
}
