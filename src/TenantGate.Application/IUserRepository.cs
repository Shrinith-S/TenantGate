using TenantGate.Domain.Entities;

namespace TenantGate.Application;

public interface IUserRepository
{
    Task<User?> GetByEntraObjectIdAsync(
        Guid tenantId,
        string entraObjectId);
}