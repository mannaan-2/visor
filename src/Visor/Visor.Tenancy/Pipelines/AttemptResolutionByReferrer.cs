using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Visor.Data.MySql.Abstractions;
using Visor.Tenancy.Abstractions;

namespace Visor.Tenancy.Tenancy.Pipelines
{
    [Obsolete]
    public class AttemptResolutionByReferrer 
    {
        private readonly RequestDelegate _next;

        public AttemptResolutionByReferrer( RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantRepository tenantRepository, ITenantContext tenantContext)
        {
            if (!tenantContext.Resolved)
            {
                var referrer = context.Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(referrer))
                {
                    var uriReferer = new Uri(referrer);
                    var tenant = tenantRepository.FindByHostName(uriReferer.Host);
                    if (tenant != null && tenant.Active)
                    {
                        tenantContext.Set(tenant.Key);
                    }
                }
            }
            await _next(context);
        }
    }
}
