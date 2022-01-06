using Microsoft.Extensions.DependencyInjection;

namespace Waves.Core.Base.Interfaces;

/// <summary>
/// Waves framework interface for "Startup" class.
/// </summary>
public interface IWavesStartup
{
    /// <summary>
    /// Configure services.
    /// </summary>
    /// <param name="services">Service collection.</param>
    void ConfigureServices(IServiceCollection services);
}
