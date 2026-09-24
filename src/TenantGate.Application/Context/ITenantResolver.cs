namespace TenantGate.Application.Context;

public interface ITenantResolver
{
    Task<Guid> ResolveTenantIdAsync();
}