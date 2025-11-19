using HexInz.Application.Contracts.Ports;

namespace HexInz.Infrastructure.Core.Persistence;

public class UnitOfWork(IDataContext dataContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dataContext.SaveChangesAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return dataContext.SaveChanges();
    }
}