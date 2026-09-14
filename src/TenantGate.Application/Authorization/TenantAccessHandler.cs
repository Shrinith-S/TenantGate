using Microsoft.AspNetCore.Authorization;
using TenantGate.Application.Services;

namespace TenantGate.Api.Authorization;

public class TenantAccessHandler : AuthorizationHandler<TenantAccessRequirement>
{
    private readonly ITenantAuthorizationService _tenantAuthorizationService;

    public TenantAccessHandler(ITenantAuthorizationService tenantAuthorizationService)
    {
        _tenantAuthorizationService = tenantAuthorizationService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TenantAccessRequirement requirement)
    {
        var authorized =
            await _tenantAuthorizationService.IsCurrentUserAuthorizedAsync();

        if (authorized)
        {
            context.Succeed(requirement);
        }
    }
}