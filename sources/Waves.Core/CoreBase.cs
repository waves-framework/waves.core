using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Pastel;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;
using Waves.Utils.Serialization;
using Color = System.Drawing.Color;

namespace Waves.Core
{
    /// <summary>
    ///     Core abstraction.
    /// </summary>
    public abstract class CoreBase : WavesObject, IWavesCore
    {
        private readonly Color _consoleDateTimeColor = System.Drawing.Color.FromArgb(128, 128, 128);
        private readonly Color _consoleSenderColor = System.Drawing.Color.FromArgb(192, 192, 192);
        private readonly Stopwatch _loadingWatch = new Stopwatch();
        private readonly List<IWavesMessage> _pendingMessages = new List<IWavesMessage>();

        /// <summary>
        ///     Gets service loader.
        /// </summary>
        private ServiceLoader ServiceLoader { get; } = new ServiceLoader();

        /// <summary>
        ///     Gets instance of logging service.
        /// </summary>
        private ILoggingService LoggingService { get; set; }

        /// <summary>
        ///     Gets instance of container service.
        /// </summary>
        private IContainerService ContainerService { get; set; }

        /// <inheritdoc />
        public WavesCoreStatus Status { get; protected set; }

        /// <inheritdoc />
        [Reactive]
        public IWavesConfiguration Configuration { get; protected set; }

        /// <inheritdoc />
        [Reactive]
        public ICollection<IWavesService> Services { get; } = new List<IWavesService>();

        /// <inheritdoc />
        [Reactive]
        public Dictionary<string, bool> InitializedServices { get; } = new Dictionary<string, bool>();

        /// <inheritdoc />
        public virtual void Start()
        {
            if (Status == WavesCoreStatus.Running
            || Status == WavesCoreStatus.Starting)
                return;
            
            try
            {
                StartLoadingWatch();
                InitializeConfiguration();
                InitializeServices();
                RegisterServices();
                BuildContainer();

                Status = WavesCoreStatus.Running;

                WriteLog(new WavesMessage("Starting",
                    $"{Name} started successfully.",
                    Name,
                    WavesMessageType.Success));

                StopLoadingWatch();
            }
            catch (Exception e)
            {
                StopLoadingWatch();
                
                WriteLog(new WavesMessage(
                    "Starting",
                    $"Error occured while starting {Name}.",
                    Name,
                    e,
                    true));
            }
        }

        /// <inheritdoc />
        public virtual void Stop()
        {
            if (Status == WavesCoreStatus.Stopped ||
                Status == WavesCoreStatus.Stopping)
                return;
            
            try
            {
                StartLoadingWatch();

                SaveConfiguration();

                foreach (var service in Services) 
                    service.Dispose();

                WriteLog(new WavesMessage(
                    "Stopping",
                    $"{Name} stopped successfully.", 
                    Name,
                    WavesMessageType.Success));

                Status = WavesCoreStatus.Stopped;
                
                StopLoadingWatch();
            }
            catch (Exception e)
            {
                StopLoadingWatch();
                
                WriteLog(new WavesMessage(
                    "Core stop",
                    $"Error occured while stopping {Name}.",
                    Name,
                    e,
                    true));
            }
        }

        /// <inheritdoc />
        public virtual void SaveConfiguration()
        {
            try
            {
                foreach (var service in Services)
                {
                    try
                    {
                        service.SaveConfiguration();
                    }
                    catch (Exception)
                    {
                        throw new Exception("Error saving \"" + service.Name + "\" configuration.");
                    }
                }

                CheckConfigurationDirectory();

                var fileName = Path.Combine(
                    Environment.CurrentDirectory,
                    "config",
                    "core.config");

                if (File.Exists(fileName))
                    File.Delete(fileName);

                Json.WriteToFile(fileName, Configuration);
            }
            catch (Exception e)
            {
                WriteLog(new WavesMessage("Saving configuration",
                    "Error configuration saving:\r\n" + e,
                    Name,
                    WavesMessageType.Error));
            }
        }

        /// <inheritdoc />
        public virtual T GetInstance<T>() where T : class
        {
            try
            {
                if (ContainerService == null) throw new NullReferenceException("Container service was not registered.");

                return ContainerService.GetInstance<T>();
            }
            catch (Exception e)
            {
                WriteLog(new WavesMessage("Getting instance",
                    "Error getting instance:\r\n" + e,
                    Name,
                    WavesMessageType.Error));

                return null;
            }
        }

        /// <inheritdoc />
        public virtual void RegisterInstance<T>(T instance) where T : class
        {
            try
            {
                if (ContainerService == null)
                {
                    throw new NullReferenceException("Container service was not registered.");
                }

                if (instance.GetType().GetInterfaces().Contains(typeof(IWavesService)))
                {
                    var service = (IWavesService) instance;
                    RegisterService(service);
                    Services.Add(service);

                    return;
                }

                ContainerService.RegisterInstance(instance);
            }
            catch (Exception e)
            {
                WriteLog(new WavesMessage("Register instance",
                    "Error registering instance:\r\n" + e,
                    Name,
                    WavesMessageType.Error));
            }
        }

        /// <inheritdoc />
        public virtual void WriteLog(string text)
        {
#if DEBUG
            Console.WriteLine(text);
#endif

            var message = new WavesMessage(
                string.Empty,
                text,
                Name,
                WavesMessageType.Information);

            OnMessageReceived(this, message);

            CheckLoggingService();

            if (CheckLoggingService())
            {
                LoggingService.WriteTextToLog(text);

                return;
            }

            _pendingMessages.Add(message);
        }

        /// <inheritdoc />
        public virtual void WriteLog(IWavesMessage message)
        {
            var status = string.Empty;
            var statusColor = new Color();

            switch (message.Type)
            {
                case WavesMessageType.Information:
                    status = "INFO";
                    statusColor = System.Drawing.Color.LightGray;
                    break;
                case WavesMessageType.Warning:
                    status = "WARN";
                    statusColor = System.Drawing.Color.LightGoldenrodYellow;
                    break;
                case WavesMessageType.Error:
                    status = "ERROR";
                    statusColor = System.Drawing.Color.OrangeRed;
                    break;
                case WavesMessageType.Fatal:
                    status = "FATAL";
                    statusColor = System.Drawing.Color.DarkRed;
                    break;
                case WavesMessageType.Success:
                    status = "SUCCESS";
                    statusColor = System.Drawing.Color.SeaGreen;
                    break;
                case WavesMessageType.Debug:
                    status = "DEBUG";
                    statusColor = System.Drawing.Color.SandyBrown;
                    break;
            }

#if DEBUG
            Console.WriteLine(
                "[{0}] [{1}]\t{2}: {3}",
                message.DateTime
                    .ToString(CultureInfo.CurrentCulture)
                    .Pastel(_consoleDateTimeColor),
                status.Pastel(statusColor),
                message.Sender.Pastel(_consoleSenderColor),
                message.Title + " - " + message.Text);

            if (message.Exception != null)
                Console.WriteLine(message.Exception.ToString());
#endif

            OnMessageReceived(this, message);

            if (CheckLoggingService())
            {
                LoggingService.WriteMessageToLog(message);

                return;
            }

            _pendingMessages.Add(message);
        }

        /// <inheritdoc />
        public virtual void WriteLog(Exception exception, string sender, bool isFatal)
        {
            string status;
            Color statusColor;

            if (isFatal)
            {
                status = "FATAL";
                statusColor = System.Drawing.Color.DarkRed;
            }
            else
            {
                status = "ERROR";
                statusColor = System.Drawing.Color.OrangeRed;
            }

            var message = new WavesMessage(exception, false);

#if DEBUG
            Console.WriteLine(
                "[{0}] [{1}]\t{2}: {3}",
                message.DateTime
                    .ToString(CultureInfo.CurrentCulture)
                    .Pastel(_consoleDateTimeColor),
                status.Pastel(statusColor),
                message.Sender.Pastel(_consoleSenderColor),
                message.Title + " - " + message.Text);

            if (message.Exception != null)
                Console.WriteLine(message.Exception.ToString());
#endif

            OnMessageReceived(this, message);

            if (CheckLoggingService())
            {
                LoggingService.WriteExceptionToLog(exception, sender, isFatal);

                return;
            }

            _pendingMessages.Add(message);
        }

        /// <summary>
        ///     Disposes object.
        /// </summary>
        public override void Dispose()
        {
            Stop();
        }

        /// <summary>
        ///     Initializes core configuration.
        /// </summary>
        private void InitializeConfiguration()
        {
            try
            {
                CheckConfigurationDirectory();

                var fileName = Path.Combine(
                    Environment.CurrentDirectory,
                    "config",
                    "core.config");

                Configuration = File.Exists(fileName)
                    ? Json.ReadFile<WavesConfiguration>(fileName)
                    : new WavesConfiguration(){Name = "Configuration"};

                Configuration.Initialize();

                Configuration.MessageReceived += OnMessageReceived;

                WriteLog(
                    new WavesMessage(
                    "Configuration initialization",
                    $"{Name} configuration initialized successfully.",
                    Name,
                    WavesMessageType.Success));
            }
            catch (Exception e)
            {
                WriteLog(
                    new WavesMessage(
                    "Configuration initialization",
                    "Error occured while configuration initialization.", 
                    Name, 
                    e, 
                    true));
            }
        }

        /// <summary>
        ///     Initializes base core services.
        /// </summary>
        private void InitializeServices()
        {
            try
            {
                ServiceLoader.MessageReceived += OnMessageReceived;
                ServiceLoader.Initialize(this);

                if (ServiceLoader.Objects == null)
                    throw new Exception("Error initializing services: services not initialized.");

                foreach (var obj in ServiceLoader.Objects) 
                    Services.Add(obj);

                WriteLog(
                    new WavesMessage(
                    "Initializing services",
                    "Services initialized successfully.",
                    "Core",
                    WavesMessageType.Success));

                WriteLog(
                    new WavesMessage(
                    "Initializing services",
                    "Number of services were loaded: " + ServiceLoader.Objects.Count(),
                    Name,
                    WavesMessageType.Information));
            }
            catch (Exception e)
            {
                WriteLog(
                    new WavesMessage(
                    "Initializing services",
                    "Error occured while initializing services:\r\n" + e,
                    Name,
                    WavesMessageType.Error));
            }
        }

        /// <summary>
        ///     Register services.
        /// </summary>
        private void RegisterServices()
        {
            try
            {
                var collection = new List<IWavesService>(Services);

                // scan for logging and container service
                foreach (var service in Services)
                {
                    if (service.GetType().GetInterfaces().Contains(typeof(IContainerService)))
                    {
                        ContainerService = (IContainerService) service;
                        collection.Remove(service);
                        RegisterService(service);
                    }

                    if (service.GetType().GetInterfaces().Contains(typeof(ILoggingService)))
                    {
                        LoggingService = (ILoggingService) service;
                        collection.Remove(service);
                        RegisterService(service);
                        WritePendingMessages();
                    }
                }

                // if container service is null, than throw exception.
                if (ContainerService == null)
                    throw new NullReferenceException("Container service not initialized or initialized with errors.");

                // if logging service is null, than throw exception.
                if (LoggingService == null)
                    throw new NullReferenceException("Logging service not initialized or initialized with errors.");

                // register other services
                foreach (var service in collection) RegisterService(service);
            }
            catch (Exception e)
            {
                WriteLog(
                    new WavesMessage(
                        "Initializing services",
                        $"Error occured while registering services.",
                        Name,
                        e,
                        false));
            }
        }

        /// <summary>
        ///     Registers service.
        /// </summary>
        /// <param name="service">Service.</param>
        private void RegisterService(IWavesService service)
        {
            try
            {
                service.MessageReceived += OnObjectMessageReceived;
                service.Initialize(this);
                service.LoadConfiguration();

                var type = service.GetType();
                var interfaces = type.GetInterfaces().ToList();

                Type result = null;

                foreach (var i in interfaces)
                {
                    var innerInterfaces = i.GetInterfaces();

                    if (!innerInterfaces.Contains(typeof(IWavesService)))
                        continue;

                    // TODO: refactor this temp fix.
                    if (i.IsGenericType)
                        continue;

                    result = i;

                    break;
                }

                if (result == null) return;

                var method = typeof(IContainerService).GetMethod("RegisterInstance");
                var genericMethod = method?.MakeGenericMethod(result);
                genericMethod?.Invoke(ContainerService, new object[] {service});

                WriteLog(new WavesMessage(
                    "Registering service",
                    $"Service {service.Name} ({service.Id}) has been registered successfully.",
                    service.Name,
                    WavesMessageType.Success));

                InitializedServices.Add(service.Name, service.IsInitialized);
            }
            catch (Exception e)
            {
                WriteLog(
                    new WavesMessage(
                        "Initializing services",
                        $"Error occured while register service {service.Name}({service.Id}):\r\n{e}",
                        Name,
                        e,
                        false));
            }
        }

        /// <summary>
        ///     Notifies when service receive message.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="message">Message.</param>
        private void OnObjectMessageReceived(object sender, IWavesMessage message)
        {
            WriteLog(message);
        }

        /// <summary>
        ///     Checks configuration directory.
        /// </summary>
        private void CheckConfigurationDirectory()
        {
            var directoryName = Path.Combine(
                Environment.CurrentDirectory,
                "config");

            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
        }

        /// <summary>
        ///     Builds container.
        /// </summary>
        private void BuildContainer()
        {
            ContainerService?.Build();
        }

        /// <summary>
        ///     Check whether logging service is loaded.
        /// </summary>
        /// <returns></returns>
        private bool CheckLoggingService()
        {
            return LoggingService != null;
        }

        /// <summary>
        ///     Writes pending log messages to log.
        /// </summary>
        private void WritePendingMessages()
        {
            foreach (var message in _pendingMessages)
                LoggingService.WriteMessageToLog(message);

            _pendingMessages.Clear();
        }

        /// <summary>
        ///     Starts loading watch.
        /// </summary>
        private void StartLoadingWatch()
        {
            if (Status == WavesCoreStatus.Starting ||
                Status == WavesCoreStatus.Stopping)
            {
                return;
            }
            
            _loadingWatch.Start();
            
            if (Status == WavesCoreStatus.NotRunning ||
                Status == WavesCoreStatus.Stopped ||
                Status == WavesCoreStatus.Failed)
            {
                WriteLog(new WavesMessage("Starting",
                    $"{Name} is starting...",
                    Name,
                    WavesMessageType.Success));
            }

            if (Status == WavesCoreStatus.Running)
            {
                WriteLog(new WavesMessage("Stopping",
                    $"{Name} is stopping...",
                    Name,
                    WavesMessageType.Success));
            }
        }

        /// <summary>
        ///     Stops loading watch.
        /// </summary>
        private void StopLoadingWatch()
        {
            if (Status == WavesCoreStatus.Starting ||
                Status == WavesCoreStatus.Stopping)
            {
                return;
            }
            
            _loadingWatch.Stop();
            
            if (Status == WavesCoreStatus.Stopped)
            {
                WriteLog(new WavesMessage($"{Name} stopped",
                    $"Time taken to stop: {Math.Round(_loadingWatch.Elapsed.TotalSeconds, 1)} seconds.",
                    Name,
                    WavesMessageType.Information));
            }
            
            if (Status == WavesCoreStatus.Failed)
            {
                WriteLog(new WavesMessage($"{Name} failed to run / stop",
                    $"Time elapsed: {Math.Round(_loadingWatch.Elapsed.TotalSeconds, 1)} seconds.",
                    Name,
                    WavesMessageType.Information));
            }

            if (Status == WavesCoreStatus.Running)
            {
                WriteLog(new WavesMessage($"{Name} started",
                    $"Time taken to start: {Math.Round(_loadingWatch.Elapsed.TotalSeconds, 1)} seconds.",
                    Name,
                    WavesMessageType.Information));
            }
        }
    }
}