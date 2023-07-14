using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base;

/// <summary>
/// Abstraction for observable / configurable plugin.
/// </summary>
public abstract class WavesObservableConfigurablePlugin :
    WavesObservableConfigurableObject,
    IWavesObservableConfigurablePlugin
{
    /// <summary>
    /// Creates new instance of <see cref="WavesObservableConfigurablePlugin"/>.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    /// <param name="logger">Logger.</param>
    protected WavesObservableConfigurablePlugin(
        IConfiguration configuration,
        ILogger<WavesObservableConfigurablePlugin> logger)
        : base(configuration, logger)
    {
    }
}
