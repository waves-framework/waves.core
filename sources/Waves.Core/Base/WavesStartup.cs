using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Splat;
using Waves.Core.Base.Interfaces;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Waves.Core.Base;

/// <summary>
/// Abstract waves framework startup class.
/// </summary>
public abstract class WavesStartup : IWavesStartup
{
    /// <inheritdoc />
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging(config => config.ClearProviders().AddConsole().SetMinimumLevel(LogLevel.Debug));

        // TODO: load plugins.
        ConfigureAdditionalServices(services);
    }

    /// <summary>
    /// Configures additional services.
    /// </summary>
    /// <param name="services">Services collection.</param>
    protected virtual void ConfigureAdditionalServices(IServiceCollection services)
    {
        // TODO: add your services here.
    }
}
