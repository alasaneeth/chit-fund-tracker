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

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new Repository<User>(context);
        Customers = new Repository<Customer>(context);
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
        => _context.Dispose();
}