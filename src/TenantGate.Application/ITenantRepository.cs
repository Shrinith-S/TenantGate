using TenantGate.Domain.Entities;

namespace TenantGate.Application;

public interface ITenantRepository
{
    Task<IReadOnlyList<Tenant>> GetActiveTenantsAsync(
        string entraTenantId);

    Task<Tenant?> GetActiveTenantByEntraTenantIdAsync(
        string entraTenantId);
}