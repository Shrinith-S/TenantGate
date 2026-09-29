using Microsoft.EntityFrameworkCore;
using TenantGate.Application.Context;
using TenantGate.Domain.Entities;

namespace TenantGate.Infrastructure.Persistence;

public class TenantGateDbContext : DbContext
{
    private readonly ICurrentTenant _currentTenant;

    public TenantGateDbContext(
        DbContextOptions<TenantGateDbContext> options,
        ICurrentTenant currentTenant)
        : base(options)
    {
        _currentTenant = currentTenant;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<PrivilegeSet> PrivilegeSets => Set<PrivilegeSet>();
    public DbSet<RolePrivilegeSet> RolePrivilegeSets => Set<RolePrivilegeSet>();
    public DbSet<FunctionalPrivilege> FunctionalPrivileges => Set<FunctionalPrivilege>();

    public DbSet<PrivilegeSetPrivilege> PrivilegeSetPrivileges => Set<PrivilegeSetPrivilege>();

    public DbSet<TechnicalPrivilege> TechnicalPrivileges => Set<TechnicalPrivilege>();

    public DbSet<FunctionalTechnicalPrivilege> FunctionalTechnicalPrivileges => Set<FunctionalTechnicalPrivilege>();


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

            entity.HasQueryFilter(
                x => x.TenantId == _currentTenant.TenantId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Code)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId);

            entity.HasIndex(x => new
            {
                x.TenantId,
                x.Code
            })
            .IsUnique();

            entity.HasQueryFilter(
                x => x.TenantId == _currentTenant.TenantId);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(x => new
            {
                x.UserId,
                x.RoleId
            });

            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(
                x => x.TenantId == _currentTenant.TenantId);
        });

        modelBuilder.Entity<PrivilegeSet>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Code)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId);

            entity.HasIndex(x => new
            {
                x.TenantId,
                x.Code
            })
            .IsUnique();

            entity.HasQueryFilter(
                x => x.TenantId == _currentTenant.TenantId);
        });

        modelBuilder.Entity<RolePrivilegeSet>(entity =>
        {
            entity.HasKey(x => new
            {
                x.RoleId,
                x.PrivilegeSetId
            });

            entity.HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.PrivilegeSet)
                .WithMany()
                .HasForeignKey(x => x.PrivilegeSetId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(
                x => x.TenantId == _currentTenant.TenantId);
        });

        modelBuilder.Entity<FunctionalPrivilege>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Code)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId);

            entity.HasIndex(x => new
            {
                x.TenantId,
                x.Code
            })
            .IsUnique();

            entity.HasQueryFilter(
                x => x.TenantId == _currentTenant.TenantId);
        });

        modelBuilder.Entity<PrivilegeSetPrivilege>(entity =>
        {
            entity.HasKey(x => new
            {
                x.PrivilegeSetId,
                x.FunctionalPrivilegeId
            });

            entity.HasOne(x => x.PrivilegeSet)
                .WithMany()
                .HasForeignKey(x => x.PrivilegeSetId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.FunctionalPrivilege)
                .WithMany()
                .HasForeignKey(x => x.FunctionalPrivilegeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(
                x => x.TenantId == _currentTenant.TenantId);
        });

        modelBuilder.Entity<FunctionalTechnicalPrivilege>(entity =>
        {
            entity.HasKey(x => new
            {
                x.FunctionalPrivilegeId,
                x.TechnicalPrivilegeId
            });

            entity.HasOne(x => x.FunctionalPrivilege)
                .WithMany()
                .HasForeignKey(x => x.FunctionalPrivilegeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.TechnicalPrivilege)
                .WithMany()
                .HasForeignKey(x => x.TechnicalPrivilegeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasQueryFilter(
                x => x.TenantId == _currentTenant.TenantId);
        });

        modelBuilder.Entity<TechnicalPrivilege>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Code)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.ApiScope)
                .HasMaxLength(200)
                .IsRequired();

            entity.HasOne<Tenant>()
                .WithMany()
                .HasForeignKey(x => x.TenantId);

            entity.HasIndex(x => new
            {
                x.TenantId,
                x.Code
            })
            .IsUnique();

            entity.HasQueryFilter(
                x => x.TenantId == _currentTenant.TenantId);
        });
    }
}