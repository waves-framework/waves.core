using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Waves.Core.Base.Interfaces;

/// <summary>
/// Waves framework interface for "Startup" class.
/// </summary>
public interface IWavesStartup
{
    /// <summary>
    /// Gets configuration.
    /// </summary>
    IConfiguration Configuration { get; }

    /// <summary>
    /// Configure services.
    /// </summary>
    /// <param name="context">Host builder context.</param>
    /// <param name="services">Service collection.</param>
    void ConfigureServices(HostBuilderContext context, IServiceCollection services);
}
