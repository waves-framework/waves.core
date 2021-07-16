using Waves.Core.Base.Interfaces;

namespace Waves.Core.Plugins.Services.Interfaces
{
    /// <summary>
    /// Interface for service that gets performance counters.
    /// </summary>
    public interface IWavesPerformanceCounterService : IWavesService
    {
        /// <summary>
        /// Gets update delay.
        /// </summary>
        int UpdateDelay { get; set; }

        /// <summary>
        /// Gets CPU load by process.
        /// </summary>
        int ProcessCpuLoad { get; }

        /// <summary>
        /// Gets total CPU load.
        /// </summary>
        int TotalCpuLoad { get; }

        /// <summary>
        /// Gets available RAM.
        /// </summary>
        long AvailableRam { get; }
    }
}
