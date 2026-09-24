namespace TenantGate.Api.Middleware;

public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantInitialization(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<TenantInitializationMiddleware>();
    }
}