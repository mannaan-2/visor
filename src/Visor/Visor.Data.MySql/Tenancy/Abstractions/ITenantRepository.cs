using System.Collections.Generic;
using Visor.Data.MySql.Abstractions;

namespace Visor.Data.MySql.Abstractions
{
    public interface ITenantRepository
    {
        IList<ITenant> GetAllTenants();
        IList<ITenant> GetActiveTenants();
        bool SetStatus(int id, bool flag);
        ITenant FindById(int id);
        ITenant FindByHostName(string host);
        ITenant FindByKey(string key);
        ITenant CreateOrUpdate(ITenant tenantValues);

    }
}
