namespace HexInz.Core.Ports;

public interface IDataContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}