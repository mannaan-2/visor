using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Visor.Data.MySql.Abstractions;
using Visor.Data.MySql.Identity.Entities;

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
            base.OnModelCreating(builder);
        }
    }
}
