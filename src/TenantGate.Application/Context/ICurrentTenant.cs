namespace TenantGate.Application.Context;

public interface ICurrentTenant
{
    Guid TenantId { get; }

    void SetTenant(Guid tenantId);
}