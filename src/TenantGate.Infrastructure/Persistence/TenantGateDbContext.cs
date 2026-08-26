using Microsoft.EntityFrameworkCore;
using TenantGate.Domain.Entities;

namespace TenantGate.Infrastructure.Persistence;

public class TenantGateDbContext : DbContext
{
    public TenantGateDbContext(
        DbContextOptions<TenantGateDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EntraTenantId)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.HasIndex(x => x.EntraTenantId)
                .IsUnique();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EntraObjectId)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(320);

            entity.Property(x => x.DisplayName)
                .HasMaxLength(200);

            entity.HasOne(x => x.Tenant)
                .WithMany()
                .HasForeignKey(x => x.TenantId);

            entity.HasIndex(x => new
            {
                x.TenantId,
                x.EntraObjectId
            })
            .IsUnique();
        });
    }
}