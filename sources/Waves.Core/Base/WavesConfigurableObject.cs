using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Waves.Core.Base.Attributes;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base;

/// <summary>
/// Abstraction for configurable object.
/// </summary>
public abstract class WavesConfigurableObject :
    WavesInitializableObject,
    IWavesConfigurableObject
{
    private readonly Dictionary<string, string> _configurations;

    /// <summary>
    /// Creates new instances of <see cref="WavesConfigurableObject"/>.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    protected WavesConfigurableObject(IConfiguration configuration)
    {
        // initialize configuration
        var attribute = GetType().GetCustomAttributes(true).SingleOrDefault(x => x is WavesObjectAttribute);
        if (attribute is not WavesObjectAttribute wavesObjectAttribute)
        {
            return;
        }

        var pluginName = wavesObjectAttribute.Name;
        _configurations = new Dictionary<string, string>();
        var configurations = configuration.GetSection(pluginName);
        if (configurations != null)
        {
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

                _configurations.Add(key, value);
            }
        }
    }

    /// <inheritdoc />
    protected override Task RunInitializationAsync()
    {
        return LoadConfigurationAsync();
    }

    /// <summary>
    /// Loads configuration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected virtual Task LoadConfigurationAsync()
    {
        var pluginType = GetType();
        var properties = pluginType.GetProperties();
        foreach (var property in properties)
        {
            var attributes = property.GetCustomAttributes(true);
            foreach (var attribute in attributes)
            {
                if (attribute is not WavesPropertyAttribute)
                {
                    continue;
                }

                var value = GetConfigurationValue(property.Name);

                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                var result = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(this, result);
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets configuration value.
    /// </summary>
    private string GetConfigurationValue(string name)
    {
        return _configurations[name];
    }
}
