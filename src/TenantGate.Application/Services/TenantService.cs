namespace TenantGate.Application.Services;

public class TenantService : ITenantService
{
    private readonly ITenantRepository _tenantRepository;

    public TenantService(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<IReadOnlyList<TenantDto>> GetTenantsAsync()
    {
        var tenants = await _tenantRepository.GetActiveTenantsAsync();

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