# HexInz

HexInz is a modular .NET 9.0 application built on hexagonal architecture principles with runtime module loading capabilities. It provides a flexible plugin-based architecture that allows infrastructure adapters to be loaded dynamically at runtime, enabling database-agnostic implementations and runtime configuration of data persistence mechanisms.

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

The solution implements a hexagonal (ports and adapters) architecture with the following layers:

### Domain Layer
- **HexInz.Domain**: Contains core business logic and domain models
- Houses the primary business entities and logic independent of external concerns
- Represents the core business domain without any infrastructure dependencies

### Application Layer
- **HexInz.Application**: Contains use cases and application services that orchestrate domain logic
- Defines application-specific business rules and workflows
- Acts as an intermediary between the domain and the infrastructure layers

### Ports Layer (Abstractions)
- **HexInz.Ports**: Defines abstractions and contracts (ports) that the domain layer depends on
- Key interface: `IDataContext` for data persistence abstraction
- Ensures domain independence from specific infrastructure implementations
- Follows the Dependency Inversion Principle by having domain depend on abstractions

### Infrastructure Layer
- **HexInz.Infrastructure.Core**: Core framework with module loading capabilities
- **HexInz.Infrastructure.EF.MySQL**: MySQL Entity Framework adapter implementation
- **HexInz.Infrastructure.EF.SQLServer**: SQL Server Entity Framework adapter implementation

#### Infrastructure Core Components
- **Module Assembly Loading Context**: Custom `AssemblyLoadContext` for dynamic module loading
- **Module Registry**: Manages loaded assemblies and module definitions
- **Module Initializer**: Handles post-registration module initialization
- **Configuration Management**: Handles configuration binding for database and other settings

## Project Structure

```
HexInz/
├── HexInz.Domain/                    # Domain layer with business logic
│   ├── HexInz.Domain.csproj          # Domain project file
│   └── Root.cs                       # Namespace root (empty file)
├── HexInz.Application/              # Application layer with use cases
│   ├── HexInz.Application.csproj     # Application project file
│   └── Root.cs                       # Namespace root (empty file)
├── HexInz.Ports/                    # Ports (abstractions) for the domain
│   ├── HexInz.Ports.csproj           # Ports project file
│   ├── IDataContext.cs               # Data context abstraction
│   └── Root.cs                       # Namespace root (empty file)
├── HexInz.Infrastructure.Core/      # Core infrastructure framework
│   ├── Constants.cs                  # Configuration constants
│   ├── HexInz.Infrastructure.Core.csproj # Core infrastructure project file
│   ├── Root.cs                       # Namespace root (empty file)
│   ├── Configurations/               # Configuration classes
│   │   └── DatabaseConfigOptions.cs  # Database configuration options
│   ├── ModulesManager/              # Module management system
│   │   ├── Contracts/               # Module interface contracts
│   │   │   └── IAmModule.cs          # Module interface contract
│   │   ├── ModuleAssemblyLoadContext.cs # Custom assembly loader
│   │   ├── ModuleInitializer.cs      # Module initialization logic
│   │   ├── ModuleLoader.cs           # Module loading logic
│   │   ├── ModuleManagerExtensions.cs # Service collection extensions
│   │   └── ModuleRegistry.cs         # Module registry management
│   └── SystemConsoleLogger/         # Custom console logging
│       └── InzConsole.cs             # Structured console output
├── HexInz.Infrastructure.EF.MySQL/  # MySQL Entity Framework adapter
│   ├── HexInz.Infrastructure.EF.MySQL.csproj # MySQL adapter project file
│   ├── EntityFrameworkMySqlModule.cs # MySQL module implementation
│   ├── MySqlDataContext.cs           # MySQL-specific data context
│   └── Root.cs                       # Namespace root (empty file)
├── HexInz.Infrastructure.EF.SQLServer/ # SQL Server Entity Framework adapter
│   ├── HexInz.Infrastructure.EF.SQLServer.csproj # SQL Server adapter project file
│   └── Root.cs                       # Namespace root (empty file)
├── HexInz.Runner/                   # Application orchestration and startup
│   ├── HexInz.Runner.csproj          # Runner project file
│   └── HexInzApp.cs                  # Application building and initialization
├── HexInz.WebAPI/                   # Main web API entry point
│   ├── HexInz.WebAPI.csproj          # Web API project file
│   ├── Program.cs                    # Web API startup logic
│   ├── appsettings.json              # Configuration settings
│   └── appsettings.Development.json  # Development-specific settings
├── HexInz.sln                        # Visual Studio solution file
└── README.md                         # This documentation file
```

## Module System

### Dynamic Module Loading
- Plugin-based architecture with runtime loading of modules
- Uses `AssemblyLoadContext` for module isolation and dependency management
- Configuration-driven module loading from `appsettings.json`
- Supports module-specific dependency resolution through `.deps.json` files

### Module Architecture
- Modules implement the `IAmModule` interface to register services and initialization logic
- Automatic service registration with dependency injection container
- Support for module initialization after service registration
- Each module can provide its own implementation of ports

### IAmModule Interface
```csharp
public interface IAmModule
{
    void RegisterServices(IServiceCollection services, IConfiguration configuration);
    void InitializeServices(IServiceProvider services, IConfiguration configuration);
}
```

Modules must implement this interface to participate in the dependency injection and initialization lifecycle.

### Assembly Loading Context
The `ModuleAssemblyLoadContext` handles:
- Loading assemblies from specific paths at runtime
- Proper dependency resolution using `AssemblyDependencyResolver`
- Sharing common dependencies with the host application
- Preventing duplicate assembly loading in memory

## Configuration

The application uses configuration to specify which modules to load:

```json
{
  "Modules": [
    "HexInz.Domain",
    "HexInz.Application", 
    "HexInz.Ports",
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

### Assembly Loading Context
- Uses custom `ModuleAssemblyLoadContext` for proper dependency resolution
- Checks the default context first to share common dependencies like EF Core, ASP.NET Core
- Handles unmanaged DLL resolution for native dependencies
- Prevents loading duplicate assemblies into memory

### Dependency Resolution
- Leverages `AssemblyDependencyResolver` to resolve dependencies from `.deps.json` files
- Configures `CopyLocalLockFileAssemblies=true` to ensure all dependencies are available for plugin loading
- Resolves dependencies in the correct order to avoid conflicts

### Logging System
- Custom `InzConsole` class provides structured logging with color-coded outputs
- Supports different levels of logging (headline, success, warning, error)
- Designed for use where `ILogger` interfaces cannot be injected
- Provides visual hierarchy for better debugging and monitoring

### Data Context Abstraction
- `IDataContext` interface abstracts data persistence operations
- Supports both synchronous and asynchronous SaveChanges operations
- Implementations are bound to specific database providers at runtime
- Enables database-agnostic application logic

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- MySQL for MySQL adapter support
- SQL Server for SQL Server adapter support

### Building the Solution

1. Clone the repository
2. Navigate to the solution directory
3. Navigate to the WebAPI project folder
4. Use dotnet user-secrets to save the database connection string securely
   ```shell
   # Run this once
   dotent user-secrets init
   # Note that you will have to update this command with the correct connection string based on your local setup  
   dotent user-secrets set "Database:ConnectionString" "Host=host;Port=port;Database=db;Username=user;Password=pass"
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

### Configuration

You can modify the modules loaded by editing `appsettings.json` in the `HexInz.WebAPI` project:

```json
{
  "Modules": [
    "HexInz.Domain",
    "HexInz.Application",
    "HexInz.Ports", 
    "HexInz.Infrastructure.Core",
    "HexInz.Infrastructure.EF.MySQL"  // Use "HexInz.Infrastructure.EF.SQLServer" for SQL Server
  ]
}
```

## Development Guidelines

### Adding New Infrastructure Adapter

To create a new infrastructure adapter:

1. Create a new project in the solution
2. In the `.csproj` file of the new project, set the value of the tag `CopyLocalLockFileAssemblies` to true
3. Add package references
4. Create a module class that implements `IAmModule`
5. Update the `Modules` configuration in `appsettings.json` with the new project's module name


### Creating Domain Modules
- Place domain logic in `HexInz.Domain`
- Keep business rules independent of infrastructure concerns
- Use interfaces in `HexInz.Ports` to define dependencies
- Ensure domain models holds no infrastructure-specific attributes

### Creating Application Services
- Place application services in `HexInz.Application`
- Define use cases and orchestrate domain logic
- Use dependency injection to get domain services and ports
- Maintain application-specific business rules

## Project Status

This solution demonstrates a modular, plugin-based architecture with runtime module loading capabilities. Key features include:

- ✅ Hexagonal architecture with clear separation of concerns
- ✅ Runtime module loading with dependency resolution
- ✅ Multiple database provider support
- ✅ Dynamic configuration of loaded modules
- ✅ Proper assembly isolation and loading
- ✅ Complete dependency injection integration

The architecture is suitable for applications that need to load infrastructure adapters dynamically based on configuration, making the system highly flexible and maintainable.

## Contributing

1. Create a feature branch (`git checkout -b feature/amazing-feature`)
2. Commit your changes (`git commit -m 'Add some amazing feature'`)
3. Push to the branch (`git push origin feature/amazing-feature`)
4. Open a Pull Request