namespace TenantGate.Domain.Entities;

public class FunctionalTechnicalPrivilege
{
    public Guid FunctionalPrivilegeId { get; set; }

    public Guid TechnicalPrivilegeId { get; set; }

    public Guid TenantId { get; set; }

    public FunctionalPrivilege FunctionalPrivilege { get; set; } = null!;

    public TechnicalPrivilege TechnicalPrivilege { get; set; } = null!;
}