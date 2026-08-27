using Microsoft.EntityFrameworkCore;
using TenantGate.Domain.Entities;

namespace TenantGate.Infrastructure.Persistence;

public static class TenantGateDbSeeder
{
    public static async Task SeedAsync(TenantGateDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Tenants.AnyAsync())
        {
            return;
        }

        var tenants = new[]
        {
            new Tenant
            {
                Id = Guid.NewGuid(),
                EntraTenantId = "DEV-TENANT-A",
                Name = "Tenant A",
                IsActive = true
            },
            new Tenant
            {
                Id = Guid.NewGuid(),
                EntraTenantId = "DEV-TENANT-B",
                Name = "Tenant B",
                IsActive = true
            },
            new Tenant
            {
                Id = Guid.NewGuid(),
                EntraTenantId = "DEV-TENANT-C",
                Name = "Tenant C",
                IsActive = true
            }
        };

        await context.Tenants.AddRangeAsync(tenants);
        await context.SaveChangesAsync();
    }
}