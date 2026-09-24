using TenantGate.Application.Context;

namespace TenantGate.Api.Context;

public class CurrentTenant : ICurrentTenant
{
    private Guid? _tenantId;

    public Guid TenantId =>
        _tenantId ?? throw new InvalidOperationException(
            "Current tenant has not been initialized.");

    public void SetTenant(Guid tenantId)
    {
        _tenantId = tenantId;
    }
}