using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenantGate.Application.Services;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "TenantAccess")]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;
    private readonly ITenantAuthorizationService _tenantAuthorizationService;

    public TenantsController(
    ITenantService tenantService,
    ITenantAuthorizationService tenantAuthorizationService)
    {
        _tenantService = tenantService;
        _tenantAuthorizationService = tenantAuthorizationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTenants()
    {
        var tenants = await _tenantService.GetTenantsAsync();

        return Ok(tenants);
    }

    [HttpGet("authorization-debug")]
    public async Task<IActionResult> AuthorizationDebug()
    {
        var authorized =
            await _tenantAuthorizationService
                .IsCurrentUserAuthorizedAsync();

        return Ok(new
        {
            Authorized = authorized
        });
    }
}