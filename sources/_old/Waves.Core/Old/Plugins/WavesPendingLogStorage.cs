using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Waves.Core.Base;
using Waves.Core.Base.Interfaces;
using Waves.Core.Old.Plugins.Interfaces;

namespace Waves.Core.Old.Plugins
{
    /// <summary>
    /// Pending log message storage.
    /// </summary>
    public class WavesPendingLogStorage : WavesPlugin, IWavesPendingLogStorage
    {
        private bool _isRecordingAvailable = true;
        
        /// <summary>
        /// Creates new instance of <see cref="WavesPendingLogStorage"/>.
        /// </summary>
        public WavesPendingLogStorage()
        {
            PendingMessages = new ConcurrentQueue<IWavesMessageObject>();
        }
        
        /// <inheritdoc />
        public ConcurrentQueue<IWavesMessageObject> PendingMessages { get; }

        /// <inheritdoc />
        public Task Push(IWavesMessageObject message)
        {
            if (_isRecordingAvailable)
            {
                PendingMessages.Enqueue(message);
            }
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<ReadOnlyCollection<IWavesMessageObject>> GetAll()
        {
            var result = new ReadOnlyCollection<IWavesMessageObject>(PendingMessages.ToList());
            _isRecordingAvailable = false;
            PendingMessages.Clear();
            return Task.FromResult(result);
        }
    }
}