using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Visor.Data.MySql.Abstractions;
using Visor.Data.MySql.Identity.Entities;
using Visor.Tenancy.Abstractions;

namespace Visor.Data.MySql.Identity
{
    public class IdentityContext :
       TenantedIdContext<IdentityContext, ApplicationUser, ApplicationRole, string, ApplicationUserClaim, ApplicationUserRole,
           ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>
    {
        //One key per tenant otherwise OnModelCreating runs only once due to caching hence the filter key is shared amongst all tenants
        public IdentityContext(DbContextOptions<IdentityContext> options, ITenantContext tenantContext)
             : base(options, tenantContext)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<ApplicationRole>(entity => entity.Property(m => m.Id).HasMaxLength(85));

            builder.Entity<ApplicationUserClaim>(entity => entity.Property(m => m.Id).HasMaxLength(85));
            builder.Entity<ApplicationRoleClaim>(entity => entity.Property(m => m.Id).HasMaxLength(85));

            builder.Entity<ApplicationUserLogin>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<ApplicationUserLogin>(entity => entity.Property(m => m.ProviderKey).HasMaxLength(85));
            builder.Entity<ApplicationUserLogin>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));

            builder.Entity<ApplicationUserRole>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<ApplicationUserRole>(entity => entity.Property(m => m.RoleId).HasMaxLength(85));

            builder.Entity<ApplicationUserToken>(entity => entity.Property(m => m.UserId).HasMaxLength(85));
            builder.Entity<ApplicationUserToken>(entity => entity.Property(m => m.Name).HasMaxLength(85));
            builder.Entity<ApplicationUserToken>(entity => entity.Property(m => m.LoginProvider).HasMaxLength(85));
            base.OnModelCreating(builder);
        }
    }
}
