using HexInz.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace HexInz.Adapter.EntityFramework.MySQL;

public class MySqlDataContext(DbContextOptions options) : DbContext(options), IDataContext
{
    public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public new int SaveChanges()
    {
        throw new NotImplementedException();
    }
}