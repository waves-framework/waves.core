using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waves.Core.Base;
using Waves.Core.Base.Attributes;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Extensions;
using Waves.Core.Plugins.Services;
using Waves.Core.Plugins.Services.Interfaces;

namespace Waves.Core
{
    /// <summary>
    ///     Core abstraction.
    /// </summary>
    public class Core :
        WavesObject,
        IWavesCore
    {
        private readonly object _pendingMessagesLocker = new object();
        private readonly List<IWavesMessageObject> _pendingMessages = new List<IWavesMessageObject>();

        private DateTime _tmpDateTime = DateTime.Now;

        /// <inheritdoc />
        public event EventHandler<IWavesMessageObject> MessageReceived;

        /// <inheritdoc />
        public CoreStatus Status { get; protected set; } = CoreStatus.NotRunning;

        /// <summary>
        ///     Gets instance of container service.
        /// </summary>
        public IWavesContainerService ContainerService { get; private set; }

        /// <summary>
        /// Gets instance of configuration service.
        /// </summary>
        public IWavesConfigurationService ConfigurationService { get; private set; }

        /// <summary>
        ///     Gets service loader.
        /// </summary>
        protected WavesTypeLoaderService<WavesPluginAttribute> PluginTypeLoaderService { get; set; }

        /// <summary>
        ///     Gets instance of logging service.
        /// </summary>
        protected ICollection<IWavesLogService> LoggingServices { get; set; } = new List<IWavesLogService>();

        /// <inheritdoc />
        public override string ToString()
        {
            return "Core";
        }

        /// <inheritdoc />
        public async Task StartAsync()
        {
            if (Status == CoreStatus.Running ||
                Status == CoreStatus.Starting)
            {
                return;
            }

            try
            {
                await WriteLogAsync(
                    "Starting",
                    $"Starting core...",
                    this,
                    WavesMessageType.Information);

                _tmpDateTime = DateTime.Now;

                Status = CoreStatus.Starting;

                await InitializePluginsAsync();

                Status = CoreStatus.Running;

                await WriteLogAsync(
                    "Starting",
                    $"Core started successfully.",
                    this,
                    WavesMessageType.Success);

                var time = DateTime.Now - _tmpDateTime;

                await WriteLogAsync(
                    "Starting",
                    $"Time taken to start: {Math.Round(time.TotalSeconds, 1)} sec.",
                    this,
                    WavesMessageType.Information);
            }
            catch (Exception e)
            {
                await WriteLogAsync(
                    e,
                    this,
                    true);
            }
        }

        /// <inheritdoc />
        public async Task StopAsync()
        {
            if (Status
                is CoreStatus.Stopping
                or CoreStatus.Failed
                or CoreStatus.NotRunning
                or CoreStatus.Stopped)
            {
                return;
            }

            try
            {
                await WriteLogAsync(
                    "Stopping",
                    $"Stopping core...",
                    this,
                    WavesMessageType.Information);

                _tmpDateTime = DateTime.Now;

                Status = CoreStatus.Stopping;

                var configurationService = await GetInstanceAsync<IWavesConfigurationService>();
                await configurationService.SaveConfigurationsAsync();

                Status = CoreStatus.Stopped;

                await WriteLogAsync(
                    "Stopping",
                    $"Core stopped successfully.",
                    this,
                    WavesMessageType.Success);

                var time = DateTime.Now - _tmpDateTime;

                await WriteLogAsync(
                    "Stopping",
                    $"Time taken to stop: {Math.Round(time.TotalSeconds, 1)} sec.",
                    this,
                    WavesMessageType.Information);
            }
            catch (Exception e)
            {
                await WriteLogAsync(
                    e,
                    this,
                    true);
            }
        }

        /// <inheritdoc />
        public async Task BuildContainerAsync()
        {
            if (ContainerService != null)
            {
                await ContainerService?.BuildAsync();
                await InitializeLoggingServiceAsync();
            }
        }

        /// <inheritdoc />
        public async Task<T> GetInstanceAsync<T>()
            where T : class
        {
            if (ContainerService == null ||
                !ContainerService.IsInitialized)
            {
                throw new NullReferenceException("Container service was not initialized.");
            }

            return await ContainerService.GetInstanceAsync<T>();
        }

        /// <inheritdoc />
        public async Task<T> GetInstanceAsync<T>(object key)
            where T : class
        {
            if (ContainerService == null ||
                !ContainerService.IsInitialized)
            {
                throw new NullReferenceException("Container service was not initialized.");
            }

            return await ContainerService.GetInstanceAsync<T>(key);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetInstancesAsync<T>()
            where T : class
        {
            if (ContainerService == null ||
                !ContainerService.IsInitialized)
            {
                throw new NullReferenceException("Container service was not initialized.");
            }

            return await ContainerService.GetInstancesAsync<T>();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetInstancesAsync<T>(object key)
            where T : class
        {
            if (ContainerService == null ||
                !ContainerService.IsInitialized)
            {
                throw new NullReferenceException("Container service was not initialized.");
            }

            return await ContainerService.GetInstancesAsync<T>(key);
        }

        /// <inheritdoc />
        public async Task WriteLogAsync(string text)
        {
            var message = new WavesTextMessage(
                text,
                string.Empty,
                this);

            if (!LoggingServices.Any())
            {
                lock (_pendingMessagesLocker)
                {
                    _pendingMessages.Add(message);
                }
            }
            else
            {
                foreach (var service in LoggingServices)
                {
                    await service.WriteLogAsync(text);
                }
            }

            OnMessageReceived(message);
        }

        /// <inheritdoc />
        public async Task WriteLogAsync(IWavesMessageObject message)
        {
            if (!LoggingServices.Any())
            {
                lock (_pendingMessagesLocker)
                {
                    _pendingMessages.Add(message);
                }
            }
            else
            {
                if (message.Type == WavesMessageType.Fatal)
                {
                    // TODO: Fatal handling.
                }

                foreach (var service in LoggingServices)
                {
                    await service.WriteLogAsync(message);
                }
            }

            OnMessageReceived(message);
        }

        /// <inheritdoc />
        public async Task WriteLogAsync(string title, string text, IWavesObject sender, WavesMessageType type)
        {
            var message = new WavesTextMessage(
                text,
                title,
                sender,
                type);

            await WriteLogAsync(message);
        }

        /// <inheritdoc />
        public async Task WriteLogAsync(Exception exception, IWavesObject sender, bool isFatal = false)
        {
            var message = new WavesExceptionMessage(
                sender,
                exception,
                isFatal);

            await WriteLogAsync(message);
        }

        /// <summary>
        /// Callback when message received.
        /// </summary>
        /// <param name="e">Message argument.</param>
        protected virtual void OnMessageReceived(IWavesMessageObject e)
        {
            MessageReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Initializes plugins.
        /// </summary>
        private async Task InitializePluginsAsync()
        {
            await InitializeTypeLoadingServiceAsync();
            await InitializeContainerServiceAsync(PluginTypeLoaderService.Types);
        }

        /// <summary>
        /// Initializes logging service.
        /// </summary>
        private async Task InitializeLoggingServiceAsync()
        {
            var list = await ContainerService.GetInstancesAsync<IWavesLogService>();

            if (list == null)
            {
                throw new NullReferenceException("There are no initialized logging services.");
            }

            foreach (var plugin in list)
            {
                try
                {
                    var service = plugin;
                    if (service == null)
                    {
                        continue;
                    }

                    LoggingServices.Add(service);
                    await service.InitializeAsync();
                }
                catch (Exception e)
                {
                    await WriteLogAsync(e, this);
                }
            }

            await WritePendingMessagesAsync();
        }

        /// <summary>
        /// Initializes container service.
        /// </summary>
        /// <param name="types">List of types.</param>
        private async Task InitializeContainerServiceAsync(IEnumerable<Type> types)
        {
            try
            {
                var list = types.ToList();
                var containers = list.Where(type => type.GetInterfaces().Contains(typeof(IWavesContainerService))).ToList();

                if (containers.Count == 0)
                {
                    throw new Exception("No containers were registered. Please check that the corresponding assembly is in the output directory.");
                }

                if (containers.Count > 1)
                {
                    throw new Exception("Multiple container services were registered. Please leave one you want to use.");
                }

                var containerType = containers.FirstOrDefault();
                if (containerType == null)
                {
                    throw new NullReferenceException("Unknown error occured while container service initialization.");
                }

                var constructor = containerType.GetConstructor(new[] { typeof(IWavesCore), typeof(IWavesTypeLoaderService) });
                if (constructor == null)
                {
                    throw new NullReferenceException("Unknown error occured while container service initialization.");
                }

                list.Remove(containerType);

                var instance = (IWavesContainerService)constructor.Invoke(new object[] { this, PluginTypeLoaderService });

                ContainerService = instance;
                await ContainerService.InitializeAsync();
                await ContainerService.RegisterSingleInstanceAsync(instance);
                await ContainerService.RegisterSingleInstanceAsync<IWavesTypeLoaderService>(PluginTypeLoaderService);
                await ContainerService.RegisterSingleInstanceAsync<IWavesCore>(this);
            }
            catch (Exception e)
            {
                await WriteLogAsync(e, this, true);
            }
        }

        /// <summary>
        ///     Writes pending log messages to log.
        /// </summary>
        private Task WritePendingMessagesAsync()
        {
            lock (_pendingMessagesLocker)
            {
                foreach (var message in _pendingMessages)
                {
                    WriteLogAsync(message).FireAndForget();
                }

                _pendingMessages.Clear();
            }

            return Task.CompletedTask;
        }

#pragma warning disable SA1124 // Do not use regions
        #region TypeLoading

        /// <summary>
        /// Initializes plugins service.
        /// </summary>
        private async Task InitializeTypeLoadingServiceAsync()
        {
            try
            {
                PluginTypeLoaderService = new WavesTypeLoaderService<WavesPluginAttribute>(this);
                await PluginTypeLoaderService.InitializeAsync();
            }
            catch (Exception e)
            {
                await WriteLogAsync(e, this, true);
            }
        }
        #endregion
    }
}
