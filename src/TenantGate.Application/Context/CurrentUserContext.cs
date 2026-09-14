using Microsoft.AspNetCore.Http;
using TenantGate.Application.Context;

namespace TenantGate.Api.Context;

public class CurrentUserContext : ICurrentUserContext
{
    private const string TenantIdClaim =
        "http://schemas.microsoft.com/identity/claims/tenantid";

    private const string ObjectIdClaim =
        "http://schemas.microsoft.com/identity/claims/objectidentifier";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserContext(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid EntraTenantId
    {
        get
        {
            var value = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst(TenantIdClaim)?
                .Value;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new UnauthorizedAccessException(
                    "Tenant ID was not found in the JWT.");
            }

            if (!Guid.TryParse(value, out var tenantId))
            {
                throw new UnauthorizedAccessException(
                    "Tenant ID in the JWT is not a valid GUID.");
            }

            return tenantId;
        }
    }

    public Guid EntraObjectId
    {
        get
        {
            var value = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst(ObjectIdClaim)?
                .Value;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new UnauthorizedAccessException(
                    "Object ID was not found in the JWT.");
            }

            if (!Guid.TryParse(value, out var objectId))
            {
                throw new UnauthorizedAccessException(
                    "Object ID in the JWT is not a valid GUID.");
            }

            return objectId;
        }
    }
}