﻿using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Visor.Data.MySql.Abstractions;

namespace Visor.Data.MySql.Tenancy.Pipelines
{
    public class AttemptResolutionByReferrer 
    {
        private readonly RequestDelegate _next;

        public AttemptResolutionByReferrer( RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantRepository tenantRepository, ITenantContext tenantContext)
        {
            if (tenantContext.Resolved)
                await _next(context);
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
            await _next(context);
        }
    }
}
