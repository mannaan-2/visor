namespace Visor.Tenancy.Abstractions
{
    public interface ITenantContext
    {
        string Key { get; }
        bool Resolved { get; }
        void Set(string key);
    }
}
