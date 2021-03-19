using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Visor.Data.MySql.Abstractions;
using Visor.Tenancy.Abstractions;

namespace Visor.Tenancy.Tenancy.Pipelines
{
    public class AttemptResolutionByQueryString 
    {
        private readonly RequestDelegate _next;

        public AttemptResolutionByQueryString( RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantRepository tenantRepository, ITenantContext tenantContext)
        {
            if (!tenantContext.Resolved)
            {
                var key = context.Request.Query[Visor.Tenancy.Abstractions.Constants.TenantQueryStringParam].ToString();
                var tenant = tenantRepository.FindByKey(key);
                if (tenant != null && tenant.Active)
                {
                    tenantContext.Set(tenant.Key);
                }
            }
            await _next(context);
        }
    }
}
