using Microsoft.Extensions.Logging;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base;

/// <summary>
/// Interface for observable / initializable plugin.
/// </summary>
public abstract class WavesObservableInitializablePlugin :
    WavesObservableInitializableObject,
    IWavesPlugin
{
    /// <summary>
    /// Creates new instance of <see cref="WavesObservableInitializablePlugin"/>.
    /// </summary>
    /// <param name="logger">Logger.</param>
    protected WavesObservableInitializablePlugin(ILogger<WavesObservableInitializableObject> logger)
        : base(logger)
    {
    }
}
