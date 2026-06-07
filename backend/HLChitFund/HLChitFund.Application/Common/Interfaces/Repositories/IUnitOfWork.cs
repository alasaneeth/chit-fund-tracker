using HLChitFund.Application.Common.Interfaces.Repositories;

namespace HLChitFund.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<Domain.Entities.User> Users { get; }
    IRepository<Domain.Entities.Customer> Customers { get; }
    IRepository<Domain.Entities.ChitGroup> ChitGroups { get; }
    IRepository<Domain.Entities.Enrollment> Enrollments { get; }
    Task<int> SaveChangesAsync();
}