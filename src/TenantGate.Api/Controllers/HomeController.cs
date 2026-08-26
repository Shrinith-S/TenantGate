using Microsoft.AspNetCore.Mvc;

namespace TenantGate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "Welcome to TenantGate",
            timestamp = DateTime.UtcNow
        });
    }
}