namespace Visor.Tenancy.Abstractions
{
    public interface ITenantedDbContext
    {
        string TenantKey { get; }
    }
}