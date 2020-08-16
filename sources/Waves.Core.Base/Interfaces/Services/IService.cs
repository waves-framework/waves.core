using System;

namespace Waves.Core.Base.Interfaces.Services
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
        void Initialize(ICore core);

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