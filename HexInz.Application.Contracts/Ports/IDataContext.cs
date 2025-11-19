namespace HexInz.Application.Contracts.Ports;

public interface IDataContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}