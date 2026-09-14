using Microsoft.EntityFrameworkCore;
using TenantGate.Application;
using TenantGate.Domain.Entities;
using TenantGate.Infrastructure.Persistence;

namespace TenantGate.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly TenantGateDbContext _dbContext;

    public UserRepository(TenantGateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByEntraObjectIdAsync(
        Guid tenantId,
        string entraObjectId)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.TenantId == tenantId &&
                x.EntraObjectId == entraObjectId &&
                x.IsActive);
    }
}