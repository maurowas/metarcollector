using Microsoft.Extensions.Configuration;

namespace Utviklertest.Infrastructure;

public static class ConfigurationEx
{
    public static string ThrowWhenNotSet(this IConfiguration config, string key) =>
        config[key] ?? throw new ConfigurationNotFoundException(key);
}

public class ConfigurationNotFoundException : Exception
{
    public ConfigurationNotFoundException(string key) : base(key)
    { }
}