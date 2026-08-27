using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TenantGate.Infrastructure.Persistence;

namespace TenantGate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly TenantGateDbContext _dbContext;

    public TenantsController(TenantGateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetTenants()
    {
        var tenants = await _dbContext.Tenants
            .AsNoTracking()
            .Select(x => new
            {
                x.Id,
                x.EntraTenantId,
                x.Name,
                x.IsActive
            })
            .ToListAsync();

        return Ok(tenants);
    }
}