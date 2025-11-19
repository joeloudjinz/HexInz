# Architectural Decision Record: Resolving Type Identity in Modular Assembly Loading

**Status:** Decided & Implemented
**Date:** 2025-11-18

## 1. Context and Problem Statement

Our system is designed with a modular architecture to promote separation of concerns and enable flexible deployment of features. The core of this design is a module loader that dynamically loads assemblies specified in a configuration file (`appsettings.json`) at startup.

Each module can participate in service registration and initialization by implementing a shared `IAmModule` interface.

### The Initial Implementation

Modules were loaded using a custom `ModuleAssemblyLoadContext` for each assembly:

```csharp
// In HexInz.Infrastructure.Core/ModulesManager/ModuleLoader.cs

// ...
foreach (var moduleName in moduleNames)
{
    var assemblyPath = /* ... resolves path ... */;
    if (File.Exists(assemblyPath))
    {
        // Each module gets its own isolated loading context.
        var loadContext = new ModuleAssemblyLoadContext(assemblyPath);
        var assembly = loadContext.LoadFromAssemblyPath(assemblyPath);
        ModuleRegistry.Add(assembly);
    }
    // ...
}
```

The `ModuleRegistry` would then instantiate the `IAmModule` implementation from the loaded assembly:

```csharp
// In HexInz.Infrastructure.Core/ModulesManager/ModuleRegistry.cs

// ...
var types = assembly.GetTypes()
    .Where(t => t.GetInterfaces().Any(ti => ti.FullName!.Equals(typeof(IAmModule).FullName))
        // ...
    ).ToList();

// ...
// This line would fail for the CoreInfrastructureModule
ModuleDefinitionsMap.Add(key, Activator.CreateInstance(types.First()) as IAmModule ?? throw new Exception($"Could not cast type {types.First().Name} to IAmModule"));
```

### The Issue

This implementation resulted in a critical runtime exception when loading the `HexInz.Infrastructure.Core` module itself:

```
System.Exception: Could not cast type CoreInfrastructureModule to IAmModule
```

This error was confusing because debugging confirmed that the `CoreInfrastructureModule` class *did* in fact implement the `IAmModule` interface. The problem was specific to loading the assembly that *defined* the shared interface.

## 2. Root Cause Analysis: Type Identity and AssemblyLoadContext

The root cause is a subtle but fundamental aspect of the .NET runtime: **type identity**. A type is uniquely identified not just by its name and namespace, but also by the assembly in which it is defined. Furthermore, if the same assembly is loaded into two different `AssemblyLoadContext` (ALC) instances, the types within them are considered distinct and incompatible.

This is exactly what was happening:

1.  **Default Load:** The host application (`HexInz.WebAPI`) starts. It has a project reference to `HexInz.Infrastructure.Core`. The .NET runtime loads `HexInz.Infrastructure.Core.dll` into the **default ALC**. The `IAmModule` interface known to the main application is from this context. Let's call it `IAmModule_DefaultContext`.

2.  **Dynamic Module Load:** Our `ModuleLoader` iterates through the configured modules. When it gets to `"HexInz.Infrastructure.Core"`, it creates a **new, isolated `ModuleAssemblyLoadContext`** and loads a *second copy* of `HexInz.Infrastructure.Core.dll` into it. The interface type loaded here is `IAmModule_CustomContext`.

3.  **The Type Mismatch:** The `ModuleRegistry` code, which runs in the default ALC, knows only about `IAmModule_DefaultContext`. When it instantiates `CoreInfrastructureModule` from the custom ALC, that object implements `IAmModule_CustomContext`.

The cast `(IAmModule_DefaultContext) instanceOfCoreInfrastructureModule` therefore fails, because the object's type hierarchy does not include `IAmModule_DefaultContext`. They are two different, incompatible types from the runtime's perspective.

## 3. Considered Options

### Option 1: Dedicated Contracts Assembly (Chosen)

This approach refactors the architecture to place all shared contracts and abstractions into a separate, dedicated assembly (e.g., `HexInz.Core.Contracts`).

*   **Implementation:**
    1.  Create a new class library project: `HexInz.Core.Contracts`.
    2.  Move the `IAmModule` interface definition into this new project.
    3.  All projects that either implement or consume these contracts (e.g., `HexInz.Infrastructure.Core`, `HexInz.Infrastructure.EF.MySQL`, `HexInz.Runner`) add a project reference to `HexInz.Core.Contracts`.
    4.  Crucially, `HexInz.Core.Contracts` is **not** listed in the `Modules` configuration file, as it is a compile-time dependency, not a dynamically loaded plugin.

*   **Pros:**
    *   **Architecturally Sound:** Creates an explicit and clear boundary between shared abstractions and their concrete implementations.
    *   **Robust & Maintainable:** Eliminates ambiguity. It's immediately obvious to developers where shared code should reside. This pattern scales well as the system grows.
    *   **Simplified Loading:** Relies on the standard .NET dependency resolution and does not require complex custom `AssemblyLoadContext` logic.

*   **Cons:**
    *   **Minor Upfront Refactoring:** Requires creating a new project and adjusting existing project references.

### Option 2: An "Intelligent" AssemblyLoadContext

This approach involves modifying the custom `ModuleAssemblyLoadContext` to be aware of shared dependencies and defer their loading to the default context.

*   **Implementation:** Override the `Load(AssemblyName assemblyName)` method in `ModuleAssemblyLoadContext` to first check if the assembly is already loaded in `AssemblyLoadContext.Default`. If it is, return the existing instance; otherwise, load it from the module's path.

*   **Pros:**
    *   **Quick Fix:** Solves the immediate problem without restructuring the solution's projects.

*   **Cons:**
    *   **Implicit and Brittle:** The "contract" of what is shared is implicitly defined by what the host application happens to reference. This can be difficult to reason about and may break in subtle ways if dependency chains change.
    *   **Increased Complexity:** Hides important architectural logic inside a low-level infrastructure component, making the system harder to understand and debug.

## 4. Decision and Rationale

We have chosen to implement **Option 1: Dedicated Contracts Assembly**.

While Option 2 provides a faster solution to the immediate bug, it introduces "magic" into the assembly loading process that obscures the system's architecture. The long-term health, clarity, and maintainability of the codebase are paramount.

**The chosen approach is superior because it makes the architectural intent explicit.** The existence of a `HexInz.Core.Contracts` project clearly communicates that there is a set of shared abstractions that form the backbone of the modular system. This aligns with well-established patterns like Clean/Onion Architecture, where abstractions are owned by the core layers.

This decision favors long-term architectural integrity over a short-term tactical fix, ensuring the system remains understandable and scalable for future development.