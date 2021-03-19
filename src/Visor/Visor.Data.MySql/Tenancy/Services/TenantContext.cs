using Microsoft.AspNetCore.Http;
using System;
using Visor.Tenancy.Abstractions;

namespace Visor.Data.MySql.Tenancy.Services
{
    public class TenantContext : ITenantContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string Key => _httpContextAccessor?.HttpContext?.Items?[Visor.Tenancy.Abstractions.Constants.TenantQueryStringParam]?.ToString();

        public bool Resolved => !string.IsNullOrEmpty(Key);
        public void Set(string key)
        {
            var context = _httpContextAccessor?.HttpContext;
            if (context == null)
                return;
            if (string.IsNullOrEmpty(key))
            {
                context.Response.Cookies.Delete(Visor.Tenancy.Abstractions.Constants.TenantCookie);
            }
            else
            {
                var option = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(60)
                };
                context.Response.Cookies.Append(Visor.Tenancy.Abstractions.Constants.TenantCookie, key, option);
            }
            if (_httpContextAccessor?.HttpContext?.Items == null)
                return;
            _httpContextAccessor.HttpContext.Items[Visor.Tenancy.Abstractions.Constants.TenantQueryStringParam] = key;
        }
    }


}
