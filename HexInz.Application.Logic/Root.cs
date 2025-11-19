using HexInz.ModuleManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HexInz.Core.Application;

public class ApplicationLogicModule: IAmModule
{
    public IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // If this module depends on MediatR (a hard dependency), it can be registered here
        // Command and query handlers can be registered into the DI from here
        return services;
    }

    public IServiceProvider InitializeServices(IServiceProvider services, IConfiguration configuration)
    {
        // If this module depends on MediatR (a hard dependency), it can be initialized here
        return services;
    }
}