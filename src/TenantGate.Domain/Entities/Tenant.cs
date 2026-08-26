namespace TenantGate.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }

    public string EntraTenantId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}