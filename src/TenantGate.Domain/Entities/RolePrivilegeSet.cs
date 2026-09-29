namespace TenantGate.Domain.Entities;

public class RolePrivilegeSet
{
    public Guid RoleId { get; set; }

    public Guid PrivilegeSetId { get; set; }

    public Guid TenantId { get; set; }

    public Role Role { get; set; } = null!;

    public PrivilegeSet PrivilegeSet { get; set; } = null!;
}