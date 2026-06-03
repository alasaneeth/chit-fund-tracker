using HLChitFund.Application.Common.Interfaces.Repositories;

namespace HLChitFund.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Domain.Entities.User> Users { get; }
    IRepository<Domain.Entities.Customer> Customers { get; }
    Task<int> SaveChangesAsync();
}