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
    string entraObjectId)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.EntraObjectId == entraObjectId &&
                x.IsActive);
    }

    public async Task<IReadOnlyList<User>> GetActiveAsync()
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == userId &&
                x.IsActive);
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);

        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);

        await _dbContext.SaveChangesAsync();
    }
}