using Microsoft.Extensions.Configuration;
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
    protected WavesConfigurablePlugin(IConfiguration configuration)
        : base(configuration)
    {
    }
}
