using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base;

/// <summary>
/// Interface for configurable plugins.
/// </summary>
public abstract class WavesConfigurablePlugin :
    WavesConfigurableObject,
    IWavesConfigurablePlugin
{
    /// <summary>
    /// Creates new instance of <see cref="WavesConfigurablePlugin"/>.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    /// <param name="logger">Logger.</param>
    protected WavesConfigurablePlugin(
        IConfiguration configuration,
        ILogger<WavesConfigurablePlugin> logger)
        : base(configuration, logger)
    {
    }
}
