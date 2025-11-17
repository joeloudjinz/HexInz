using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HexInz.Infrastructure.Core.ModulesManager.Contracts;

public interface IAmModule
{
    void RegisterServices(IServiceCollection services, IConfiguration configuration);
    void InitializeServices(IServiceProvider services, IConfiguration configuration);
}