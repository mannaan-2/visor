using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Visor.Data.MySql.Utilities;
using Visor.Tenancy.Abstractions;

namespace Visor.Data.MySql.Abstractions
{
    public abstract class TenantedContext<TContext> : DbContext, ITenantedDbContext where TContext : TenantedContext<TContext>
    {
        public string TenantKey { get; }


        public TenantedContext(DbContextOptions<TContext> options, ITenantContext tenantContext)
            : base(options)
        {
            TenantKey = tenantContext?.Key;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            TenantDbContexhelper.AddTenantProperty(builder, TenantKey);
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            TenantDbContexhelper.SetTenantValue(ChangeTracker, TenantKey);
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            TenantDbContexhelper.SetTenantValue(ChangeTracker, TenantKey);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
