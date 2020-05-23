using System;

namespace Ambertape.Core.Base.Interfaces
{
    /// <summary>
    ///     Base interface for all services.
    /// </summary>
    public interface IService : IObject, IDisposable
    {
        /// <summary>
        ///     Gets whether service is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        ///     Initializes service.
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Loads configuration.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        void LoadConfiguration(IConfiguration configuration);

        /// <summary>
        ///     Saves configuration.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        void SaveConfiguration(IConfiguration configuration);
    }
}