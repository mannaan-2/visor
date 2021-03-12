using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Visor.Data.MySql.Abstractions;

namespace Visor.Data.MySql.Tenancy.Pipelines
{
    internal class InitializeTenantResolutionProcessor
    {
        private readonly RequestDelegate _next;
        private readonly ITenantContext _tenantContext;

        public InitializeTenantResolutionProcessor(RequestDelegate next, ITenantContext tenantContext)
        {
            _next = next;
            _tenantContext = tenantContext;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            _tenantContext.Set(null);
            await _next(context);
        }
    }
}
