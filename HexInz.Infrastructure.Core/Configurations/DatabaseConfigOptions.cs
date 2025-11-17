namespace Infrastructure.Core.Configurations;

public class DatabaseConfigOptions
{
    public const string SectionName = "Database";
    public string ConnectionString { get; set; } = string.Empty;
}