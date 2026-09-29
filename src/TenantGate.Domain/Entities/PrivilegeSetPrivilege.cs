namespace TenantGate.Domain.Entities;

public class PrivilegeSetPrivilege
{
    public Guid PrivilegeSetId { get; set; }

    public Guid FunctionalPrivilegeId { get; set; }

    public Guid TenantId { get; set; }

    public PrivilegeSet PrivilegeSet { get; set; } = null!;

    public FunctionalPrivilege FunctionalPrivilege { get; set; } = null!;
}