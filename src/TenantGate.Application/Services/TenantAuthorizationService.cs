using TenantGate.Application.Context;

namespace TenantGate.Application.Services;

public class TenantAuthorizationService : ITenantAuthorizationService
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserContext _currentUserContext;

    public TenantAuthorizationService(
        ITenantRepository tenantRepository,
        IUserRepository userRepository,
        ICurrentUserContext currentUserContext)
    {
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
        _currentUserContext = currentUserContext;
    }

    public async Task<bool> IsCurrentUserAuthorizedAsync()
    {
        var entraTenantId =
            _currentUserContext.EntraTenantId.ToString();

        var tenant = await _tenantRepository
            .GetActiveTenantByEntraTenantIdAsync(entraTenantId);

        if (tenant == null)
        {
            return false;
        }

        var user = await _userRepository
            .GetByEntraObjectIdAsync(
                tenant.Id,
                _currentUserContext.EntraObjectId.ToString());

        return user != null;
    }
}