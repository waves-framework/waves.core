using System.Threading.Tasks;

namespace Waves.Core.Base.Interfaces;

/// <summary>
/// Interface for initializable object.
/// </summary>
public interface IWavesInitializableObject
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
