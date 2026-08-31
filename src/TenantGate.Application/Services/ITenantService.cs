namespace TenantGate.Application.Services;

public interface ITenantService
{
    Task<IReadOnlyList<TenantDto>> GetTenantsAsync();
}