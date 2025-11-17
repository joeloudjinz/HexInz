# HexInz

HexInz is a modular .NET 9.0 application built on hexagonal architecture principles with runtime module loading capabilities. It provides a flexible plugin-based architecture that allows infrastructure adapters to be loaded dynamically at runtime.

## Architecture

The solution implements a hexagonal (ports and adapters) architecture with the following layers:

### Domain Layer
- **HexInz.Domain**: Contains core business logic and domain models
- Houses the primary business entities and logic independent of external concerns

### Application Layer
- Contains use cases and application services that orchestrate domain logic

### Ports Layer
- **HexInz.Ports**: Defines abstractions and contracts (ports) that the domain layer depends on
- Key interface: `IDataContext` for data persistence abstraction

### Infrastructure Layer
- **HexInz.Infrastructure.Core**: Core framework with module loading capabilities
- **HexInz.Infrastructure.EF.MySQL**: MySQL Entity Framework adapter implementation
- **HexInz.Infrastructure.EF.SQLServer**: SQL Server Entity Framework adapter (placeholder)

### Application Runner
- **HexInz.Runner**: Orchestrates module loading and application setup
- **HexInz.WebAPI**: Main web API entry point

## Key Features

### Dynamic Module Loading
- Plugin-based architecture with runtime loading of modules
- Uses `AssemblyLoadContext` for module isolation and dependency management
- Configuration-driven module loading from `appsettings.json`

### Module System
- Modules implement the `IAmModule` interface to register services and initialization logic
- Automatic service registration with dependency injection container
- Support for module initialization after service registration

### Infrastructure Abstractions
- Data access abstractions through ports
- Multiple database provider support (MySQL and SQL Server)

## Project Structure

```
HexInz/
├── HexInz.Domain/           # Domain layer with business logic
├── HexInz.Ports/            # Ports (abstractions) for the domain
├── HexInz.Infrastructure.Core/  # Core infrastructure framework
├── HexInz.Infrastructure.EF.MySQL/  # MySQL adapter
├── HexInz.Infrastructure.EF.SQLServer/  # SQL Server adapter
├── HexInz.WebAPI/           # Web API startup and configuration
├── HexInz.Runner/           # Application runner and orchestration
└── README.md
```

## Configuration

The application uses configuration to specify which modules to load:

```json
{
  "Modules": [
    "HexInz.Domain",
    "HexInz.Ports", 
    "HexInz.Infrastructure.Core",
    "HexInz.Infrastructure.EF.MySQL"
  ]
}
```

## Module System

The module loading system works as follows:

1. **Module Discovery**: Reads module names from configuration
2. **Assembly Loading**: Dynamically loads assemblies using `ModuleAssemblyLoadContext`
3. **Service Registration**: Discovers and registers services with the DI container
4. **Module Initialization**: Initializes modules after all services are registered

### Module Interface

Modules must implement the `IAmModule` interface:

```csharp
public interface IAmModule
{
    void RegisterServices(IServiceCollection services, IConfiguration configuration);
    void InitializeServices(IServiceProvider services, IConfiguration configuration);
}
```

## Infrastructure Adapters

### Entity Framework MySQL Adapter
- Provides MySQL database connectivity using Pomelo.EntityFrameworkCore.MySql
- Implements the `IDataContext` interface
- Supports connection string configuration

### Entity Framework SQL Server Adapter
- Placeholder for SQL Server support
- Uses Microsoft.EntityFrameworkCore.SqlServer

## Getting Started

### Prerequisites
- .NET 9.0 SDK
- MySQL
- SQLServer

### Running the Application

1. Clone the repository
2. Navigate to the solution directory
3. Build the solution:
   ```bash
   dotnet build
   ```
4. Run the WebAPI project:
   ```bash
   dotnet run --project HexInz.WebAPI
   ```

### Development

- The application follows a modular approach where new infrastructure adapters can be added as modules
- Modules are loaded at runtime based on configuration
- Each module can register its own services with the dependency injection container
- Each module will have to initialize its own services

## Technical Details

### Assembly Loading Context
- Uses custom `ModuleAssemblyLoadContext` for proper dependency resolution
- Handles shared dependencies by checking the default context first
- Prevents loading duplicate assemblies into memory

### Dependency Resolution
- Leverages `AssemblyDependencyResolver` to resolve dependencies from `.deps.json` files
- Configures `CopyLocalLockFileAssemblies=true` to ensure all dependencies are available for plugin loading

### Logging
- Custom `InzConsole` class provides structured logging with color-coded outputs that should be used where ILogger can not be used
- Supports different levels of logging (headline, success, warning, error)


## Project Status
This solution demonstrates a modular, plugin-based architecture with runtime module loading capabilities. It's suitable for applications that need to load infrastructure adapters dynamically based on configuration.
