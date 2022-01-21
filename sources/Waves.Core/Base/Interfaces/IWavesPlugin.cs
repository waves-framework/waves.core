using System.Threading.Tasks;

namespace Waves.Core.Base.Interfaces;

/// <summary>
///     Interface for plugin.
/// </summary>
public interface IWavesPlugin : IWavesObject
{
    /// <summary>
    /// Gets whether plugin is initializes.
    /// </summary>
    bool IsInitialized { get; }

    /// <summary>
    /// Initializes plugin.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task InitializeAsync();
}
