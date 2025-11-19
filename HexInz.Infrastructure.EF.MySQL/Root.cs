using HexInz.Application.Contracts.Ports;
using HexInz.Infrastructure.Common.Configurations;
using HexInz.ModuleManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HexInz.Infrastructure.EF.MySQL;

public class EntityFrameworkMySqlModule : IAmModule
{
    public IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        var databaseConfigOptions = new DatabaseConfigOptions(configuration);
        services.AddDbContext<IDataContext, MySqlDataContext>(options =>
        {
            options.UseMySql(databaseConfigOptions.ConnectionString, ServerVersion.AutoDetect(databaseConfigOptions.ConnectionString));
        });
        return services;
    }

    public IServiceProvider InitializeServices(IServiceProvider services, IConfiguration configuration)
    {
        // No need for now
        Console.WriteLine("Initializing MySql Database");
        return services;
    }
}