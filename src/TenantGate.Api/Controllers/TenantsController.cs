using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenantGate.Application.Services;

namespace TenantGate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "TenantAccess")]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantsController(
        ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTenants()
    {
        var tenants = await _tenantService.GetTenantsAsync();

        return Ok(tenants);
    }
}