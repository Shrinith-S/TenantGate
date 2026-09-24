using TenantGate.Domain.Entities;

namespace TenantGate.Application;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetActiveAsync();

    Task<User?> GetByIdAsync(Guid userId);

    Task<User?> GetByEntraObjectIdAsync(
    string entraObjectId);

    Task AddAsync(User user);

    Task UpdateAsync(User user);
}