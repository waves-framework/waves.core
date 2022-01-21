using System.Threading.Tasks;

namespace Waves.Core.Base.Interfaces;

/// <summary>
///     Wrapper interface over reactive object.
/// </summary>
public interface IWavesObject
{
    /// <summary>
    /// Async loads configuration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task LoadConfigurationAsync();

    /// <summary>
    /// Async saves configuration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SaveConfigurationAsync();
}
