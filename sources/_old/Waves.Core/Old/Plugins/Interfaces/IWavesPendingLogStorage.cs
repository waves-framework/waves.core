using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Old.Plugins.Interfaces
{
    /// <summary>
    /// Interface for storage of pending log message.
    /// </summary>
    public interface IWavesPendingLogStorage
    {
        /// <summary>
        /// Gets collection of pending messages.
        /// </summary>
        ConcurrentQueue<IWavesMessageObject> PendingMessages { get; }

        /// <summary>
        /// Push message to storage.
        /// </summary>
        /// <param name="message">Message.</param>
        Task Push(IWavesMessageObject message);

        /// <summary>
        /// Get all pending messages and clears collection.
        /// </summary>
        /// <returns>Messages.</returns>
        Task<ReadOnlyCollection<IWavesMessageObject>> GetAll();
    }
}