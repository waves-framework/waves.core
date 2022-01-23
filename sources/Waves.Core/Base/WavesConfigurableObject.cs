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
    private readonly string _pluginName;
    private readonly IEnumerable<KeyValuePair<string, string>> _configurations;

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

        _pluginName = wavesObjectAttribute.Name;
        var configurations = configuration.GetSection(_pluginName);
        if (configurations != null)
        {
            _configurations = configuration.AsEnumerable();
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

                if (!string.IsNullOrEmpty(value))
                {
                    property.SetValue(this, value);
                }
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets configuration value.
    /// </summary>
    private string GetConfigurationValue(string name)
    {
        return (from configuration
            in _configurations
            let startKey = $"{_pluginName}:"
            where configuration.Key.Equals($"{startKey}{name}")
            select configuration.Value)
            .FirstOrDefault();
    }
}
