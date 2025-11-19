using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

namespace HexInz.ModuleManager;

public static class ModuleManagerExtensions
{
    public static void RegisterModules(this IServiceCollection services, IConfiguration configuration)
    {
        var moduleNames = configuration.GetSection(Constants.ModulesConfigurationLabel).Get<string[]>() ?? [];
        if (moduleNames.Length == 0) throw new Exception("No modules are specified in configuration");

        ModuleLoader.Load(moduleNames);

        foreach (var module in ModuleRegistry.LoadedModuleDefinitions) module.RegisterServices(services, configuration);
    }

    public static void InitializeModules(this IServiceProvider services, IConfiguration configuration)
    {
        if (ModuleRegistry.LoadedAssemblies.Count == 0) throw new Exception("No assemblies are loaded!");

        ModuleInitializer.Initialize(services, configuration);
    }
}