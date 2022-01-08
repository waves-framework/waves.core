using System;
using System.Threading.Tasks;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface for plugin.
    /// </summary>
    public interface IWavesPlugin : 
        IWavesObject,
        IDisposable
    {
        /// <summary>
        ///     Is plugin initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        ///     Initializes plugin.
        /// </summary>
        /// <returns>Returns initialization task.</returns>
        Task InitializeAsync();
    }
}
