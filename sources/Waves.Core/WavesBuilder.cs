using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Waves.Core.Base.Interfaces;
using Waves.Core.Extensions;

namespace Waves.Core;

/// <summary>
/// Waves builder.
/// </summary>
public static class WavesBuilder
{
    /// <summary>
    /// Default filename for settings file.
    /// </summary>
    private const string DefaultSettingsFileName = "appsettings.json";

    /// <summary>
    /// Creates default builder.
    /// </summary>
    /// <returns>Configuration builder.</returns>
    public static IConfigurationBuilder CreateDefaultBuilder() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(DefaultSettingsFileName, optional: false, reloadOnChange: true);

    /// <summary>
    /// Builds service provider.
    /// </summary>
    /// <typeparam name="TWavesStartup">Type of startup class.</typeparam>
    /// <returns>Returns service provider.</returns>
    public static IServiceCollection GetDefaultServices<TWavesStartup>()
        where TWavesStartup : IWavesStartup, new()
    {
        var startup = GenericExtensions.InvokeConstructor<TWavesStartup>();
        var serviceCollection = new ServiceCollection();
        startup.ConfigureServices(serviceCollection);
        return serviceCollection;
    }
}
