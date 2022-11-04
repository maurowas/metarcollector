using Microsoft.Extensions.Configuration;

namespace MetarCollector.Infrastructure;

public static class ConfigurationEx
{
    public static string ThrowWhenNotSet(this IConfiguration config, string key) =>
        config[key] ?? throw new ConfigurationNotFoundException(key);

    public static string GetValue(this IConfiguration config, string key, string defaultValue) =>
        string.IsNullOrWhiteSpace(config[key]) ? defaultValue : config[key]!;
}

public class ConfigurationNotFoundException : Exception
{
    public ConfigurationNotFoundException(string key) : base(key)
    { }
}