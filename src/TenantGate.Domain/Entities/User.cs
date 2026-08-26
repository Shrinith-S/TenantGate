namespace TenantGate.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string EntraObjectId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public Tenant? Tenant { get; set; }
}