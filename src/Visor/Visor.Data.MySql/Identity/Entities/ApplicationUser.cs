using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Visor.Data.MySql.Abstractions;
using Visor.Tenancy.Abstractions;

namespace Visor.Data.MySql.Identity.Entities
{
    // a copy of 20180926135602_recreateIdIndexes migration needs to run everytime this class changes
    // As it will drop the composit index and recreate on table key
    public class ApplicationUser : IdentityUser, ITenantedEntity
    {

        public string FirstName { get; set; }
        //[Required]
        public string LastName { get; set; }
    }
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {

    }
    public class ApplicationUserLogin : IdentityUserLogin<string>, ITenantedEntity
    {

    }
    public class ApplicationUserRole : IdentityUserRole<string>
    {

    }
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {

    }
    public class ApplicationUserToken : IdentityUserToken<string>
    {

    }
}
