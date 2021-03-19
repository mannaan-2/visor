using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using Visor.Data.MySql.Identity.Entities;
using Visor.Data.MySql.Utilities;
using Visor.Tenancy.Abstractions;

namespace Visor.Data.MySql.Abstractions
{
    public abstract class TenantedIdContext<TContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>, ITenantedDbContext
        where TContext : TenantedIdContext<TContext, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> where TUser : IdentityUser<TKey> where TRole : IdentityRole<TKey> where TKey : System.IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey> where TUserRole : IdentityUserRole<TKey> where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey> where TUserToken : IdentityUserToken<TKey>
    {
        public string TenantKey { get; }

        public DbSet<Tenant> Tenants { get; set; }
        public TenantedIdContext(DbContextOptions<TContext> options, ITenantContext tenantContext)
            : base(options)
        {
            TenantKey = tenantContext?.Key;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            TenantDbContexhelper.AddTenantProperty(builder, TenantKey);
            builder.Entity<Tenant>()
               .HasIndex(u => u.Key)
               .IsUnique();
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
