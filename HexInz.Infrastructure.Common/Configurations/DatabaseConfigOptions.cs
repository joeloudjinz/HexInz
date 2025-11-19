using Microsoft.Extensions.Configuration;

namespace HexInz.Infrastructure.Common.Configurations;

public class DatabaseConfigOptions
{
    public DatabaseConfigOptions(IConfiguration configuration)
    {
        configuration.GetSection(SectionName).Bind(this);
    }

    public const string SectionName = "Database";
    public string ConnectionString { get; set; } = string.Empty;
}