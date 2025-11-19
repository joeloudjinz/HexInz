using HexInz.Core.Domain.Circulation.Services;
using HexInz.ModuleManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HexInz.Core.Domain;

public class DomainModule : IAmModule
{
    public IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IOverdueFineService, OverdueFineService>();
        return services;
    }

    public IServiceProvider InitializeServices(IServiceProvider services, IConfiguration configuration)
    {
        return services;
    }
}