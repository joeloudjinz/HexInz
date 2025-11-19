# HexInz

HexInz is a modular .NET 9.0 application built on hexagonal architecture and Domain-Driven Design (DDD) principles. It features robust dynamic module loading capabilities that enable infrastructure adapters to be loaded at runtime, ensuring complete separation of concerns and module isolation while maintaining clean architecture boundaries.

## Table of Contents
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Module System](#module-system)
- [Configuration](#configuration)
- [Technical Details](#technical-details)
- [Getting Started](#getting-started)
- [Development Guidelines](#development-guidelines)
- [Project Status](#project-status)

## Architecture

The solution implements a hexagonal (ports and adapters) architecture with the following components:

### Domain Layer
- **HexInz.Domain**: Contains core business logic and domain models

### Application Layer
- **HexInz.Application.Contracts**: Contains application contracts, interfaces, and ports that define abstractions the domain layer depends on
- **HexInz.Application.Logic**: Contains application services and business logic orchestration

### Infrastructure Layer
- **HexInz.Runner**: Provides application orchestration and startup logic with service registration and initialization extensions for module management
- **HexInz.ModuleManager**: Contains module loading, registry, and initialization functionality
- **HexInz.Infrastructure.Common**: Contains common infrastructure contracts, interfaces, and configuration management
- **HexInz.Infrastructure.Core**: Contains core infrastructure components and persistence abstractions
- **HexInz.Infrastructure.EF.MySQL**: MySQL Entity Framework adapter implementation
- **HexInz.Infrastructure.EF.PostgreSQL**: PostgreSQL Entity Framework adapter implementation

## Project Structure

```
HexInz/
├── Documentations/                   # Documentation files
├── HexInz.Application.Contracts/     # Application contracts and ports
│   ├── HexInz.Application.Contracts.csproj # Application contracts project file
│   ├── Root.cs                       # Namespace root (empty file)
│   └── Ports/                        # Application-specific ports with IDataContext and IUnitOfWork
│       ├── IDataContext.cs           # Data context abstraction interface
│       └── IUnitOfWork.cs            # Unit of work pattern interface
├── HexInz.Application.Logic/         # Application logic layer (business logic orchestration)
│   ├── HexInz.Application.Logic.csproj # Application logic project file
│   └── Root.cs                       # Namespace root (empty file)
├── HexInz.Domain/                    # Domain layer with business logic
│   ├── HexInz.Domain.csproj          # Domain project file
│   ├── README.md                     # Domain layer documentation
│   ├── Root.cs                       # Namespace root (empty file)
│   ├── Circulation/                  # Circulation domain components
│   ├── Common/                       # Common domain components
│   ├── Inventory/                    # Inventory domain components
│   └── Memberships/                  # Memberships domain components
├── HexInz.Infrastructure.Common/     # Common infrastructure contracts
│   ├── HexInz.Infrastructure.Common.csproj # Common infrastructure project file
│   └── Docs/                         # Documentation for common infrastructure
├── HexInz.Infrastructure.Core/       # Core infrastructure framework (minimal implementation)
│   ├── HexInz.Infrastructure.Core.csproj # Core infrastructure project file
│   ├── Root.cs                       # Namespace root (empty file)
│   └── Persistence/                  # Persistence abstractions
├── HexInz.Infrastructure.EF.MySQL/   # MySQL Entity Framework adapter
│   ├── HexInz.Infrastructure.EF.MySQL.csproj # MySQL adapter project file
│   ├── MySqlDataContext.cs           # MySQL-specific data context
│   └── Root.cs                       # Namespace root (empty file)
├── HexInz.Infrastructure.EF.PostgreSQL/ # PostgreSQL Entity Framework adapter
│   ├── HexInz.Infrastructure.EF.PostgreSQL.csproj # PostgreSQL adapter project file
│   └── Root.cs                       # Namespace root (empty file)
├── HexInz.ModuleManager/             # Module management system
│   ├── HexInz.ModuleManager.csproj   # Module manager project file
│   ├── Constants.cs                  # Configuration constants
│   ├── IAmModule.cs                  # Core module interface definition
│   ├── InzConsole.cs                 # Custom console logging system
│   ├── ModuleAssemblyLoadContext.cs  # Custom assembly loading context
│   ├── ModuleInitializer.cs          # Module initialization logic
│   ├── ModuleLoader.cs               # Module loading implementation
│   ├── ModuleManagerExtensions.cs    # Module manager extensions
│   └── ModuleRegistry.cs             # Module registry implementation
├── HexInz.Runner/                    # Application orchestration and startup
│   ├── HexInz.Runner.csproj          # Runner project file
│   └── HexInzApp.cs                  # Application building and initialization
├── HexInz.UnitTests.Domain/          # Unit tests for domain layer
│   ├── HexInz.Domain.UnitTests.csproj # Unit test project file
│   ├── Circulation/                  # Unit tests for circulation
│   ├── Common/                       # Unit tests for common components
│   ├── Inventory/                    # Unit tests for inventory
│   └── Memberships/                  # Unit tests for memberships
├── HexInz.WebAPI/                    # Main web API entry point
│   ├── HexInz.WebAPI.csproj          # Web API project file
│   ├── Program.cs                    # Web API startup logic
│   ├── appsettings.json              # Configuration settings
│   ├── appsettings.Development.json  # Development-specific settings
│   └── Properties/                   # Project properties
├── .gitignore                        # Git ignore file
├── Directory.Build.targets           # MSBuild customizations applied to all projects
├── Directory.Packages.props          # Centralized NuGet package version management
├── HexInz.sln                        # Visual Studio solution file
└── README.md                         # This documentation file
```

## Documentations Directory

The `Documentations/` directory is intended for project documentation files, including architecture diagrams, design documents, API documentation, and other technical documentation. Currently, this directory is empty but is reserved for future documentation needs.

## Module System
- Robust plugin-based architecture with runtime loading of modules ensuring complete isolation
- Configuration-driven module loading from `appsettings.json` with dynamic dependency resolution
- Dedicated module management system in HexInz.ModuleManager project for proper lifecycle management
- Modules implement the `IAmModule` interface to register services with dependency injection container
- Support for module initialization after service registration ensuring proper startup sequence
- Each module maintains clear separation of concerns and can provide its own infrastructure implementations

### IAmModule Interface
```csharp
namespace HexInz.ModuleManager;

public interface IAmModule
{
    IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration);
    IServiceProvider InitializeServices(IServiceProvider services, IConfiguration configuration);
}
```

Modules must implement this interface to participate in the dependency injection and initialization lifecycle.


## Configuration

The application uses configuration to specify which modules to load:

```json
{
  "Modules": [
    "HexInz.Runner",
    "HexInz.ModuleManager",
    "HexInz.Domain",
    "HexInz.Application.Contracts",
    "HexInz.Application.Logic",
    "HexInz.Infrastructure.Common",
    "HexInz.Infrastructure.Core",
    "HexInz.Infrastructure.EF.MySQL"
  ]
}
```

### Module Loading Process
1. **Module Discovery**: Reads module names from the `Modules` section in configuration
2. **Assembly Loading**: Dynamically loads assemblies using `ModuleAssemblyLoadContext`
3. **Service Registration**: Discovers and registers services with the DI container via `IAmModule` implementations
4. **Module Initialization**: Initializes modules after all services are registered

## Technical Details

### Modularity and Dynamic Loading
- Custom `ModuleAssemblyLoadContext` in HexInz.ModuleManager enables robust dynamic module loading with proper isolation
- Leverages `AssemblyDependencyResolver` to resolve dependencies from `.deps.json` files
- Configures `CopyLocalLockFileAssemblies=true` to ensure all dependencies are available for plugin loading
- Prevents loading duplicate assemblies into memory while sharing common dependencies

### Separation of Concerns and Isolation
- Complete separation between domain, application, and infrastructure layers
- Modules are isolated from each other with proper dependency boundaries
- Infrastructure adapters are loaded dynamically without compile-time dependencies
- Each module maintains its own clear responsibility and scope

### Hexagonal Architecture and DDD Implementation
- Ports and adapters pattern ensures business logic remains independent of infrastructure
- Domain layer is completely isolated from external concerns
- Application layer orchestrates business workflows without infrastructure coupling
- Clean architecture principles ensure maintainable and testable code

### Module Management and Service Registration
- Automatic service registration with dependency injection container via `IAmModule` implementations
- Post-registration module initialization ensures proper lifecycle management

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- MySQL for MySQL adapter support
- PostgreSQL for PostgreSQL adapter support

### Building the Solution

1. Clone the repository
2. Navigate to the solution directory
3. Navigate to the WebAPI project folder
4. Use dotnet user-secrets to save the database connection string securely
   ```shell
   # Run this once
   dotnet user-secrets init
   # Note that you will have to update this command with the correct connection string based on your local setup
   dotnet user-secrets set "Database:ConnectionString" "Host=host;Port=port;Database=db;Username=user;Password=pass"
   # To confirm secret is added, run this command to list all the secrets
   dotnet user-secrets list
   ```
5. Build the solution:
   ```bash
   dotnet build
   ```

### Running the Application

1. Navigate to the WebAPI directory:
   ```bash
   cd HexInz.WebAPI
   ```

2. Run the application:
   ```bash
   dotnet run --launch-profile api
   ```

3. The application will start on `http://localhost:5029`

## Development Guidelines

### Adding New Infrastructure Adapter

To create a new infrastructure adapter:

1. Create a new project in the solution
2. In the `.csproj` file of the new project, set the value of the tag `IsModuleProject` to true
3. Add package references in `Directory.Packages.props` with the right versions
4. Reference the required packages specified in `Directory.Packages.props` in the `.csproj` file of the new project
5. Create a module class that implements `IAmModule` from `HexInz.ModuleManager`
6. Update the `Modules` configuration in `appsettings.json` with the new project's module name
7. Run `dotnet clean` then `dotnet build`


### Creating Application Services
- Place application contracts and ports in `HexInz.Application.Contracts`
- Place business logic orchestration and application services in `HexInz.Application.Logic`
- Use dependency injection to get domain services and ports
- Maintain application-specific business rules

## Project Status

This solution demonstrates core architectural principles with a modular, plugin-based design. Key features include:

- ✅ Hexagonal architecture with Domain-Driven Design (DDD) principles
- ✅ Robust runtime module loading with complete isolation
- ✅ Clear separation of concerns between all architectural layers
- ✅ Dynamic configuration of loaded modules at runtime
- ✅ Proper module lifecycle management and dependency injection integration
- ✅ Complete infrastructure abstraction through ports and adapters pattern
- ✅ Dedicated module management system for enhanced modularity

The architecture is suitable for applications that require maximum flexibility, maintainability, and clean separation between business logic and infrastructure concerns.

## Contributing

1. Create a feature branch (`git checkout -b feature/amazing-feature`)
2. Commit your changes (`git commit -m 'Add some amazing feature'`)
3. Push to the branch (`git push origin feature/amazing-feature`)
4. Open a Pull Request