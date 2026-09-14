namespace TenantGate.Application.Context;

public interface ICurrentUserContext
{
    Guid EntraObjectId { get; }

    Guid EntraTenantId { get; }
}