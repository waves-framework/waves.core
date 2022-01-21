using System.Threading.Tasks;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Services.Interfaces;

/// <summary>
/// Interface for configuration service.
/// </summary>
public interface IWavesConfigurationService
{
    /// <summary>
    /// Loads configuration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task LoadConfigurationsAsync();

    /// <summary>
    /// Saves configuration.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SaveConfigurationsAsync();

    /// <summary>
    /// Adds new configurable object.
    /// </summary>
    /// <param name="configurable">Configurable object.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task AddObjectAsync(IWavesObject configurable);

    /// <summary>
    /// Removes configurable object.
    /// </summary>
    /// <param name="configurable">Configurable object.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task RemoveObjectAsync(IWavesObject configurable);

    /// <summary>
    /// Gets configuration for current object.
    /// </summary>
    /// <param name="configurable">Configurable object.</param>
    /// <returns>Returns configuration.</returns>
    Task<IWavesConfiguration> GetConfigurationAsync(IWavesObject configurable);

    // /// <summary>
    // /// Imports configuration.
    // /// </summary>
    // /// <param name="fileName">Path to ZIP-file with configuration.</param>
    // /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    // Task ImportConfigurationsAsync(string fileName);
    //
    // /// <summary>
    // /// Exports configuration.
    // /// </summary>
    // /// <param name="fileName">Path to ZIP-file where configuration will be saved.</param>
    // /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    // Task ExportConfigurationsAsync(string fileName);
}
