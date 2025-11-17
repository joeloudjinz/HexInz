using HexInz.Infrastructure.Core.ModulesManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HexInz.Runner;

public static class HexInzApp
{
    public static void Build(IServiceCollection serviceCollection, IConfiguration configuration, IHostEnvironment environment)
    {
        serviceCollection.RegisterModules(configuration);
    }

    public static void Init(IServiceProvider serviceProvider, IConfiguration configuration, IHostEnvironment environment)
    {
        serviceProvider.InitializeModules(configuration);
    }

    public static void Init(IHost host)
    {
        throw new NotImplementedException();
    }

    public static void Build(IHostApplicationBuilder builder)
    {
        throw new NotImplementedException();
    }
}