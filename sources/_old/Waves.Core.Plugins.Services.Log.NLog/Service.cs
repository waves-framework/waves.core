using System;
using System.Threading.Tasks;
using NLog;
using NLog.Common;

namespace Waves.Core.Plugins.Services.Log.NLog
{
    /// <summary>
    ///     Logging service.
    /// </summary>
    [WavesService(
        "041D44C6-34C5-4F40-9155-7D1F588695AC",
        typeof(IWavesLogService))]
    public class Service :
        WavesLogServiceBase
    {
        private Logger _logger;

        /// <inheritdoc />
        public override Task InitializeAsync()
        {
            if (IsInitialized)
            {
                return Task.CompletedTask;
            }

            _logger = LogManager.GetCurrentClassLogger();

            InternalLogger.LogToConsole = false;

            IsInitialized = true;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task WriteLogAsync(string text)
        {
            if (!IsInitialized)
            {
                return Task.CompletedTask;
            }

            _logger.Info(
                "{0} {1}",
                $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}",
                text);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override async Task WriteLogAsync(IWavesMessageObject message)
        {
            if (!IsInitialized)
            {
                return;
            }

            const string text = "{0} {1} {2}: {3}";

            switch (message.Type)
            {
                case WavesMessageType.Information:
                    _logger.Info(
                        text,
                        await GetParamsAsync(message));
                    break;
                case WavesMessageType.Warning:
                    _logger.Warn(
                        text,
                        await GetParamsAsync(message));
                    break;
                case WavesMessageType.Error:
                    _logger.Error(
                        text,
                        await GetParamsAsync(message));
                    break;
                case WavesMessageType.Success:
                    _logger.Info(
                        text,
                        await GetParamsAsync(message));
                    break;
                case WavesMessageType.Debug:
                    _logger.Debug(
                        text,
                        await GetParamsAsync(message));
                    break;
                case WavesMessageType.Fatal:
                    _logger.Fatal(
                        text,
                        await GetParamsAsync(message));
                    break;
                case WavesMessageType.Verbose:
                    _logger.Fatal(
                        text,
                        await GetParamsAsync(message));
                    break;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "NLog Logging Service";
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TODO: your code for release managed resources.
            }

            // TODO: your code for release unmanaged resources.
        }

        /// <summary>
        /// Converts message to number of log params.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <returns>Returns params.</returns>
        private Task<object[]> GetParamsAsync(IWavesMessageObject message)
        {
            return Task.FromResult(new object[]
            {
                $"{message.DateTime.ToShortDateString()} {message.DateTime.ToShortTimeString()}",
                $"[{message.Type.ToDescription()}]",
                message.Sender,
                message.ToString(),
            });
        }
    }
}
