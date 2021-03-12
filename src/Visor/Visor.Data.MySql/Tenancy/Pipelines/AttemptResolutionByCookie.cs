﻿using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;
using Visor.Data.MySql.Abstractions;

namespace Visor.Data.MySql.Tenancy.Pipelines
{
    public class AttemptResolutionByCookie : IMiddleware
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly ITenantContext _tenantContext;

        public AttemptResolutionByCookie(ITenantRepository tenantRepository, ITenantContext tenantContext)
        {
            _tenantRepository = tenantRepository;
            _tenantContext = tenantContext;

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (_tenantContext.Resolved)
                await next(context);
            var key = context.Request.Cookies[Constants.TenantCookie];
            if (!string.IsNullOrEmpty(key))
            {
                var tenant = _tenantRepository.FindByKey(key);
                if (tenant != null && tenant.Active)
                {
                    _tenantContext.Set(tenant.Key);
                }
            }
            await next(context);
        }
    }
}
