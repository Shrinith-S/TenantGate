using TenantGate.Application.Context;

namespace TenantGate.Api.Middleware;

public class TenantInitializationMiddleware
{
    private readonly RequestDelegate _next;

    public TenantInitializationMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITenantResolver tenantResolver,
        ICurrentTenant currentTenant)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantId =
                await tenantResolver.ResolveTenantIdAsync();

            currentTenant.SetTenant(tenantId);
        }

        await _next(context);
    }
}