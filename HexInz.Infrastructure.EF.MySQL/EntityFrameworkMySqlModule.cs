using HexInz.Core.Ports;
using HexInz.Infrastructure.Core.Configurations;
using HexInz.Infrastructure.Core.ModulesManager.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HexInz.Adapter.EntityFramework.MySQL;

public class EntityFrameworkMySqlModule : IAmModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfigOptions = new DatabaseConfigOptions(configuration);
        services.AddDbContext<IDataContext, MySqlDataContext>(options => { options.UseMySql(databaseConfigOptions.ConnectionString, ServerVersion.AutoDetect(databaseConfigOptions.ConnectionString)); });
    }

    public void InitializeServices(IServiceProvider services, IConfiguration configuration)
    {
        // No need for now
        Console.WriteLine("Initializing MySql Database");
    }
}