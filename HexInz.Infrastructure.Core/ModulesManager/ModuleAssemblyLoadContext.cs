using System.Reflection;
using System.Runtime.Loader;
using Infrastructure.Core.SystemConsoleLogger;

namespace Infrastructure.Core.ModulesManager;

internal sealed class ModuleAssemblyLoadContext(string modulePath) : AssemblyLoadContext(isCollectible: true)
{
    private readonly AssemblyDependencyResolver _resolver = new(modulePath);

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        InzConsole.Headline("ModuleAssemblyLoadContext()");
        InzConsole.FirstLevelItem($"Module: [{modulePath}]");
        InzConsole.FirstLevelItem($"Assembly: [{assemblyName}]");

        // Before we try to load anything into our custom context, we first check if the *default* AssemblyLoadContext (the host's context)
        // has already loaded an assembly with the same name.
        // This is essential for sharing common dependencies like EF Core, ASP.NET Core, etc.
        var defaultAssembly = Default.Assemblies.FirstOrDefault(a => a.GetName().Name == assemblyName.Name);

        if (defaultAssembly != null)
        {
            // If the host already has it, we use its version.
            // This prevents loading two different versions of the same DLL into memory.
            InzConsole.SecondLevelItem($"Resolved from default assembly: [{defaultAssembly.GetName().Name}]");
            InzConsole.EndHeadline();
            return defaultAssembly;
        }

        // If the default context doesn't have it, we try to resolve it from the module's
        // own dependencies using the .deps.json file.
        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);

        if (assemblyPath != null)
        {
            InzConsole.SecondLevelItem($"Resolved From assembly path: [{assemblyPath}]");
            InzConsole.EndHeadline();
            return LoadFromAssemblyPath(assemblyPath);
        }

        InzConsole.Error($"UNRESOLVED ASSEMBLY: [{assemblyName}]");
        InzConsole.EndHeadline();
        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        return libraryPath != null ? LoadUnmanagedDllFromPath(libraryPath) : IntPtr.Zero;
    }
}