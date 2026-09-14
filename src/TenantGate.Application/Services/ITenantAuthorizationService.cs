namespace TenantGate.Application.Services;

public interface ITenantAuthorizationService
{
    Task<bool> IsCurrentUserAuthorizedAsync();
}