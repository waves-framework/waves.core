using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Old.Plugins.Services.Interfaces
{
    /// <summary>
    /// Interface for configuration service.
    /// </summary>
    public interface IWavesConfigurationService : IWavesService
    {
        /// <summary>
        /// Gets or sets path to configuration directory.
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Gets collection of configuration.
        /// </summary>
        IDictionary<Guid, IWavesConfiguration> Configurations { get; }

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
        Task AddConfigurableAsync(IWavesConfigurableObject configurable);

        /// <summary>
        /// Removes configurable object.
        /// </summary>
        /// <param name="configurable">Configurable object.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveConfigurableAsync(IWavesConfigurableObject configurable);

        /// <summary>
        /// Gets configuration for current object.
        /// </summary>
        /// <param name="configurable">Configurable object.</param>
        /// <returns>Returns configuration.</returns>
        Task<IWavesConfiguration> GetConfigurationAsync(IWavesConfigurableObject configurable);

        /// <summary>
        /// Imports configuration.
        /// </summary>
        /// <param name="fileName">Path to ZIP-file with configuration.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ImportConfigurationsAsync(string fileName);

        /// <summary>
        /// Exports configuration.
        /// </summary>
        /// <param name="fileName">Path to ZIP-file where configuration will be saved.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ExportConfigurationsAsync(string fileName);
    }
}
