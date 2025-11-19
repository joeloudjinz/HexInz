using HexInz.Application.Contracts.Ports;
using HexInz.Infrastructure.Core.Persistence;
using HexInz.ModuleManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HexInz.Infrastructure.Core;

public class CoreInfrastructureModule : IAmModule
{
    public IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

    public IServiceProvider InitializeServices(IServiceProvider services, IConfiguration configuration)
    {
        Console.WriteLine("Initializing Infrastructure Core Module");
        return services;
    }
}