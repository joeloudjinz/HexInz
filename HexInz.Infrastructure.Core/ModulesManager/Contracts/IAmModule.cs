using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Core.ModulesManager.Contracts;

public interface IAmModule
{
    void RegisterServices(IServiceCollection services, IConfiguration configuration);
    void InitializeServices(IServiceProvider services, IConfiguration configuration);
}