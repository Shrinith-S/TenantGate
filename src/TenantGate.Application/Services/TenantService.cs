using TenantGate.Application.Context;

namespace TenantGate.Application.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantContext _tenantContext;

    public TenantService(
        ITenantRepository tenantRepository,
        ITenantContext tenantContext)
    {
        _tenantRepository = tenantRepository;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<TenantDto>> GetTenantsAsync()
    {
        var entraTenantId = _tenantContext.EntraTenantId.ToString();

        var tenants = await _tenantRepository
            .GetActiveTenantsAsync(entraTenantId);

        return tenants
            .Select(x => new TenantDto
            {
                Id = x.Id,
                EntraTenantId = x.EntraTenantId,
                Name = x.Name,
                IsActive = x.IsActive
            })
            .ToList();
    }
}