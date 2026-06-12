using HLChitFund.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HLChitFund.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ChitGroup> ChitGroups => Set<ChitGroup>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Winner> Winners => Set<Winner>();
    public DbSet<Commission> Commissions => Set<Commission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Soft Delete Global Filter
        modelBuilder.Entity<User>()
            .HasQueryFilter(u => !u.IsDeleted);

        modelBuilder.Entity<Customer>()
            .HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<ChitGroup>()
            .HasQueryFilter(cg => !cg.IsDeleted);

        modelBuilder.Entity<Enrollment>()
            .HasQueryFilter(e => !e.IsDeleted);

        // Decimal Precision
        modelBuilder.Entity<ChitGroup>()
            .Property(c => c.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ChitGroup>()
            .Property(c => c.MonthlyAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ChitGroup>()
            .Property(c => c.CommissionRate)
            .HasPrecision(5, 2);

      
        modelBuilder.Entity<Payment>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<Payment>()
            .Property(p => p.AmountPaid)
            .HasPrecision(18, 2);

        // Winner
        modelBuilder.Entity<Winner>()
            .HasQueryFilter(w => !w.IsDeleted);

        modelBuilder.Entity<Winner>()
            .Property(w => w.PrizeAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Winner>()
            .Property(w => w.CommissionDeducted)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Winner>()
            .Property(w => w.NetAmount)
            .HasPrecision(18, 2);

        // Commission
        modelBuilder.Entity<Commission>()
            .HasQueryFilter(c => !c.IsDeleted);

        modelBuilder.Entity<Commission>()
            .Property(c => c.TotalAmount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Commission>()
            .Property(c => c.CommissionRate)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Commission>()
            .Property(c => c.CommissionAmount)
            .HasPrecision(18, 2);

    }
}