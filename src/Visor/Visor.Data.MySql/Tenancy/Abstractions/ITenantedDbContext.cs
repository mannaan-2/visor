namespace Visor.Data.MySql.Abstractions
{
    public interface ITenantedDbContext
    {
        string TenantKey { get; }
    }
}