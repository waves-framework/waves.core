using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    /// <param name="args">Arguments</param>
    /// <returns>Host builder.</returns>
    public static IHostBuilder CreateDefaultBuilder(string[] args) => Host.CreateDefaultBuilder(args);

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
