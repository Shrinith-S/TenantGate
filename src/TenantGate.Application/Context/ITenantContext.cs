namespace TenantGate.Application.Context;

public interface ITenantContext
{
    Guid EntraTenantId { get; }
}