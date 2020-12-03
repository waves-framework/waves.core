using System;

namespace Waves.Core.Base.Interfaces.Services
{
    /// <summary>
    ///     Base interface for all services.
    /// </summary>
    public interface IWavesService : IWavesObject
    {
        /// <summary>
        ///     Gets whether service is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        ///     Initializes service.
        /// </summary>
        void Initialize(IWavesCore core);

        /// <summary>
        ///     Loads configuration.
        /// </summary>
        void LoadConfiguration();

        /// <summary>
        ///     Saves configuration.
        /// </summary>
        void SaveConfiguration();
    }
}