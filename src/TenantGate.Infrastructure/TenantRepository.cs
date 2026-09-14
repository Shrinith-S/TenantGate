using Microsoft.EntityFrameworkCore;
using TenantGate.Application;
using TenantGate.Domain.Entities;
using TenantGate.Infrastructure.Persistence;

namespace TenantGate.Infrastructure;

public class TenantRepository : ITenantRepository
{
    private readonly TenantGateDbContext _dbContext;

    public TenantRepository(TenantGateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Tenant>> GetActiveTenantsAsync(
        string entraTenantId)
    {
        return await _dbContext.Tenants
            .AsNoTracking()
            .Where(x => x.IsActive &&
                        x.EntraTenantId == entraTenantId)
            .ToListAsync();
    }

    public async Task<Tenant?> GetActiveTenantByEntraTenantIdAsync(
    string entraTenantId)
    {
        return await _dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.IsActive &&
                x.EntraTenantId == entraTenantId);
    }
}