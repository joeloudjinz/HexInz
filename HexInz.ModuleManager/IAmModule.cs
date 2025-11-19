using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HexInz.ModuleManager;

public interface IAmModule
{
    IServiceCollection RegisterServices(IServiceCollection services, IConfiguration configuration);
    IServiceProvider InitializeServices(IServiceProvider services, IConfiguration configuration);
}