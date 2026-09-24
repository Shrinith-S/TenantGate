using TenantGate.Application;
using TenantGate.Application.Context;

namespace TenantGate.Api.Context;

public class TenantResolver : ITenantResolver
{
    private readonly ITenantContext _tenantContext;
    private readonly ITenantRepository _tenantRepository;

    public TenantResolver(
        ITenantContext tenantContext,
        ITenantRepository tenantRepository)
    {
        _tenantContext = tenantContext;
        _tenantRepository = tenantRepository;
    }

    public async Task<Guid> ResolveTenantIdAsync()
    {
        var entraTenantId =
            _tenantContext.EntraTenantId.ToString();

        var tenant = await _tenantRepository
            .GetActiveTenantByEntraTenantIdAsync(entraTenantId);

        if (tenant == null)
        {
            throw new UnauthorizedAccessException(
                "Tenant is not registered or inactive.");
        }

        return tenant.Id;
    }
}