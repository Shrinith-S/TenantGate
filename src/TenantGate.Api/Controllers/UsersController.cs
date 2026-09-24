using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenantGate.Application.Services;

namespace TenantGate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "TenantAccess")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetUsersAsync();

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(
    CreateUserRequest request)
    {
        var user = await _userService.CreateUserAsync(request);

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(
    Guid id,
    UpdateUserRequest request)
    {
        var user = await _userService
            .UpdateUserAsync(id, request);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var deleted = await _userService.DeleteUserAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}