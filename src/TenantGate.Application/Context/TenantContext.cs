using Microsoft.AspNetCore.Http;
using TenantGate.Application.Context;

namespace TenantGate.Api.Context;

public class TenantContext : ITenantContext
{
    private const string TenantIdClaim =
        "http://schemas.microsoft.com/identity/claims/tenantid";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid EntraTenantId
    {
        get
        {
            var tenantIdClaim = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst(TenantIdClaim)?
                .Value;

            if (string.IsNullOrWhiteSpace(tenantIdClaim))
            {
                throw new UnauthorizedAccessException(
                    "Tenant ID was not found in the JWT.");
            }

            if (!Guid.TryParse(tenantIdClaim, out var tenantId))
            {
                throw new UnauthorizedAccessException(
                    "Tenant ID in the JWT is not a valid GUID.");
            }

            return tenantId;
        }
    }
}