using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Waves.Core.Base.Attributes;

namespace Waves.Core.Extensions;

/// <summary>
/// Extensions for configurable object.
/// </summary>
public static class ConfigurableExtensions
{
    /// <summary>
    /// Initializes configuration for object.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="configuration">Initial configuration.</param>
    /// <returns>Configuration.</returns>
    public static Dictionary<string, string> InitializeConfiguration(object obj, IConfiguration configuration)
    {
        // initialize configuration
        var attribute = obj.GetType().GetCustomAttributes(true).SingleOrDefault(x => x is WavesObjectAttribute);
        if (attribute is not WavesObjectAttribute wavesObjectAttribute)
        {
            return null;
        }

        var pluginName = wavesObjectAttribute.Name;
        var configurationsDictionary = new Dictionary<string, string>();
        var configurations = configuration.GetSection(pluginName);
        if (configurations == null)
        {
            return configurationsDictionary;
        }

        var e = configuration.AsEnumerable();
        var startKey = $"{pluginName}:";
        foreach (var config in e)
        {
            if (!config.Key.StartsWith(startKey))
            {
                continue;
            }

            var key = config.Key.Replace(startKey, string.Empty);
            var value = config.Value;

            configurationsDictionary.Add(key, value);
        }

        return configurationsDictionary;
    }

    /// <summary>
    /// Configures object.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="configuration">Configuration.</param>
    /// <param name="logger">Logger.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static Task Configure(this object obj, Dictionary<string, string> configuration, ILogger logger = null)
    {
        var pluginType = obj.GetType();
        var properties = pluginType.GetProperties();
        foreach (var property in properties)
        {
            try
            {
                var attributes = property.GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    if (attribute is not WavesPropertyAttribute)
                    {
                        continue;
                    }

                    var value = configuration[property.Name];

                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }

                    var result = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(obj, result);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occured while loading configuration value");
            }
        }

        return Task.CompletedTask;
    }
}
