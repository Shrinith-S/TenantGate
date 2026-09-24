namespace TenantGate.Application.Services;

public class UpdateUserRequest
{
    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
}