namespace TenantGate.Application.Services;

public class CreateUserRequest
{
    public string EntraObjectId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
}