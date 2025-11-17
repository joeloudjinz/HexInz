using System.Reflection;
using HexInz.Infrastructure.Core.SystemConsoleLogger;

namespace HexInz.Infrastructure.Core.ModulesManager;

internal static class ModuleLoader
{
    public static void Load(string[] moduleNames)
    {
        LoadModuleAssemblies(moduleNames);

        if (ModuleRegistry.LoadedAssemblies.Count == 0) throw new Exception("No assemblies to load");

        ModuleRegistry.InstantiateModuleDefinitions();
    }

    private static void LoadModuleAssemblies(string[] moduleNames)
    {
        InzConsole.Headline("LoadModuleAssemblies()");

        var entryAssemblyLocation = Assembly.GetEntryAssembly()?.Location;
        InzConsole.Log($"From location: {Assembly.GetEntryAssembly()?.Location}");
        if (string.IsNullOrEmpty(entryAssemblyLocation)) throw new InvalidOperationException("Could not determine entry assembly location.");

        var entryAssemblyName = Assembly.GetEntryAssembly()?.GetName().Name;
        if (entryAssemblyName is null) throw new InvalidOperationException("Entry assembly name is null");

        var rootPath = Path.GetDirectoryName(entryAssemblyLocation)!;
        foreach (var moduleName in moduleNames)
        {
            // The main DLL for the module is expected to be in a subfolder named after the module.
            var assemblyPath = Path.Combine(rootPath, $"{moduleName}.dll").Replace(entryAssemblyName, moduleName);
            InzConsole.FirstLevelItem($"Loading assembly: [{moduleName}]");
            InzConsole.FirstLevelItem($"Path: [{assemblyPath}]");
            if (File.Exists(assemblyPath))
            {
                var loadContext = new ModuleAssemblyLoadContext(assemblyPath);
                var assembly = loadContext.LoadFromAssemblyPath(assemblyPath);

                ModuleRegistry.Add(assembly);
                InzConsole.SuccessWithNewLine("Assembly loaded");
            }
            else
            {
                throw new FileNotFoundException($"Module assembly not found at {assemblyPath}. Ensure the module project is configured correctly to copy its output.");
            }
        }
        InzConsole.EndHeadline();
    }
}