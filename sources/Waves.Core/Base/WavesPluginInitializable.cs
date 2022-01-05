using System.Threading.Tasks;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base;

/// <summary>
///     Abstract base for initializable plugins.
/// </summary>
public abstract class WavesPluginInitializable :
    WavesPlugin,
    IWavesPluginInitializable
{
    /// <inheritdoc />
    public abstract Task InitializeAsync();
}
