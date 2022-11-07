using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Waves.Core.Extensions;

/// <summary>
/// Core extensions.
/// </summary>
public static class CoreExtensions
{
    /// <summary>
    /// Configures logging.
    /// </summary>
    /// <param name="core">Core.</param>
    /// <param name="builder">Logging builder.</param>
    public static void AddLogging(this WavesCore core, Action<ILoggingBuilder> builder)
    {
        core.LoggingBuilder = builder;
    }

    /// <summary>
    /// Configures services.
    /// </summary>
    /// <param name="core">Core.</param>
    /// <param name="configureServices">Configure services action.</param>
    public static void AddServices(this WavesCore core, Action<IServiceCollection> configureServices)
    {
        core.ConfigureServices = configureServices;
    }
}
