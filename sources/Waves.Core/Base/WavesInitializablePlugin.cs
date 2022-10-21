using Microsoft.Extensions.Logging;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base;

/// <summary>
/// Initializable plugin abstraction.
/// </summary>
public abstract class WavesInitializablePlugin :
    WavesInitializableObject,
    IWavesInitializablePlugin
{
    /// <summary>
    /// Creates new instance of <see cref="WavesInitializablePlugin"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    protected WavesInitializablePlugin(ILogger<WavesInitializablePlugin> logger)
        : base(logger)
    {
    }
}
