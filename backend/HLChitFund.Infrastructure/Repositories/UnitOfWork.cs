using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.Common.Interfaces.Repositories;
using HLChitFund.Domain.Entities;
using HLChitFund.Infrastructure.Data;

namespace HLChitFund.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IRepository<User> Users { get; private set; }
    public IRepository<Customer> Customers { get; private set; }
    public IRepository<ChitGroup> ChitGroups { get; private set; }
    public IRepository<Enrollment> Enrollments { get; private set; }
    public IRepository<Payment> Payments { get; private set; }

    public IRepository<Winner> Winners { get; private set; }
    public IRepository<Commission> Commissions { get; private set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new Repository<User>(context);
        Customers = new Repository<Customer>(context);
        ChitGroups = new Repository<ChitGroup>(context);
        Enrollments = new Repository<Enrollment>(context);
        Payments = new Repository<Payment>(context);
        Winners = new Repository<Winner>(context);
        Commissions = new Repository<Commission>(context);

    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
        => _context.Dispose();
}