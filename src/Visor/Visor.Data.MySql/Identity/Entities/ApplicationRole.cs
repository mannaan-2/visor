using Microsoft.AspNetCore.Identity;
using Visor.Tenancy.Abstractions;

namespace Visor.Data.MySql.Identity.Entities
{
    // a copy of 20180926135602_recreateIdIndexes migration needs to run everytime this class changes
    // As it will drop the composit index and recreate on table key
    public class ApplicationRole : IdentityRole, ITenantedEntity
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }

    }
}