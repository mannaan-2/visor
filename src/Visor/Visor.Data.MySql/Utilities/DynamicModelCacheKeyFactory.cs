using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Visor.Data.MySql.Abstractions;

namespace Visor.Data.MySql.Utilities
{
    internal class DynamicModelCacheKeyFactory : IModelCacheKeyFactory
    {
        public object Create(DbContext context)
        {
            if (context is ITenantedDbContext dynamicContext)
            {
                return (context.GetType(), dynamicContext.TenantKey);
            }
            return context.GetType();
        }
    }
}
