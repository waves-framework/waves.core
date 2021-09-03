namespace Waves.Core.Plugins.Services
{
#if Windows
    /// <summary>
    ///     Service that gets performance counters.
    /// </summary>
    [WavesService(
        "2a4de00a-2509-44ff-a835-182ee74076e2",
        typeof(IWavesPerformanceCounterService))]
    internal class WavesPerformanceCounterService : WavesConfigurableService, IWavesPerformanceCounterService
    {
        private PerformanceCounter _cpuCounter;
        private PerformanceCounter _availableRamCounter;
        private CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        ///     Creates new instance of <see cref="WavesPerformanceCounterService" />.
        /// </summary>
        /// <param name="configurationService">Configurable service.</param>
        public WavesPerformanceCounterService(IWavesConfigurationService configurationService)
            : base(configurationService)
        {
        }

        /// <inheritdoc />
        [Reactive]
        [WavesProperty]
        public int UpdateDelay { get; set; } = 100;

        /// <inheritdoc />
        [Reactive]
        public int ProcessCpuLoad { get; private set; }

        /// <inheritdoc />
        [Reactive]
        public int TotalCpuLoad { get; private set; }

        /// <inheritdoc />
        [Reactive]
        public long AvailableRam { get; private set; }

        /// <inheritdoc />
        public override Task InitializeAsync()
        {
            if (IsInitialized)
            {
                return Task.CompletedTask;
            }

#if NET5_0
            if (OperatingSystem.IsWindows())
            {
#endif
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _availableRamCounter = new PerformanceCounter("Memory", "Available MBytes");

            RunCounter().FireAndForget();

            IsInitialized = true;
#if NET5_0
            }
#endif

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "Performance counter service";
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();

            _cpuCounter.Dispose();
            _availableRamCounter.Dispose();
        }

        /// <summary>
        ///     Runs counter.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task RunCounter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
#if NET5_0
            if (OperatingSystem.IsWindows())
            {
#endif
            var proc = Process.GetCurrentProcess();
            do
            {
                TotalCpuLoad = (int)_cpuCounter.NextValue();
                AvailableRam = (int)_availableRamCounter.NextValue();
                ProcessCpuLoad = (int)await GetCpuUsageForProcess(proc).ConfigureAwait(false);
            }
            while (!_cancellationTokenSource.IsCancellationRequested);
#if NET5_0
            }
#endif
        }

        /// <summary>
        ///     Gets cpu usage for current process.
        /// </summary>
        /// <param name="proc">Process.</param>
        /// <returns>CPU usage.</returns>
        private async Task<double> GetCpuUsageForProcess(Process proc)
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = proc.TotalProcessorTime;
            await Task.Delay(UpdateDelay);
            var endTime = DateTime.UtcNow;
            var endCpuUsage = proc.TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
            return cpuUsageTotal * 100;
        }
    }
#endif
}
