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

    }
}