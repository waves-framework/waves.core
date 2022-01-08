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
    /// <param name="args">Arguments.</param>
    /// <returns>Host builder.</returns>
    public static IHostBuilder CreateDefaultBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureHostConfiguration(configHost =>
    {
        configHost.SetBasePath(Directory.GetCurrentDirectory());
        configHost.AddJsonFile(DefaultSettingsFileName, optional: true);
        configHost.AddEnvironmentVariables(prefix: "PREFIX_");
        configHost.AddCommandLine(args);
    });

    /// <summary>
    /// Uses current startup for host builder.
    /// </summary>
    /// <param name="builder">Builder.</param>
    /// <typeparam name="TWavesStartup">Type of startup class.</typeparam>
    /// <returns>Returns host builder.</returns>
    public static IHostBuilder UseStartup<TWavesStartup>(this IHostBuilder builder)
        where TWavesStartup : IWavesStartup, new()
    {
        var startup = GenericExtensions.InvokeConstructor<TWavesStartup>();
        return builder.ConfigureServices((context, collection) =>
        {
            startup.ConfigureServices(context, collection);
        });
    }
}
