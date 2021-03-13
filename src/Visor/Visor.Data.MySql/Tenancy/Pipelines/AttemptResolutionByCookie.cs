﻿using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;
using Visor.Data.MySql.Abstractions;

namespace Visor.Data.MySql.Tenancy.Pipelines
{
    public class AttemptResolutionByCookie 
    {
        private readonly RequestDelegate _next;
        public AttemptResolutionByCookie( RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantRepository tenantRepository, ITenantContext tenantContext)
        {
            if (tenantContext.Resolved)
                await _next(context);
            var key = context.Request.Cookies[Constants.TenantCookie];
            if (!string.IsNullOrEmpty(key))
            {
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
