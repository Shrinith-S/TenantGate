namespace TenantGate.Domain.Entities
{
    public class FunctionalPrivilege
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public Guid TenantId { get; set; }

        public bool IsSystemPrivilege { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
