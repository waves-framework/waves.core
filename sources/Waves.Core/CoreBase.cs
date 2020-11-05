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
using Object = Waves.Core.Base.Object;

namespace Waves.Core
{
    /// <summary>
    ///     Core abstraction.
    /// </summary>
    public abstract class CoreBase : Object, ICore
    {
        private readonly Color _consoleDataTimeColor = Color.FromArgb(128, 128, 128);
        private readonly Color _consoleSenderColor = Color.FromArgb(192, 192, 192);
        private readonly Stopwatch _loadingWatch = new Stopwatch();
        private readonly List<IMessage> _pendingMessages = new List<IMessage>();

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
        public CoreStatus Status { get; protected set; }

        /// <inheritdoc />
        [Reactive]
        public IConfiguration Configuration { get; protected set; }

        /// <inheritdoc />
        [Reactive]
        public ICollection<IService> Services { get; protected set; } = new List<IService>();

        /// <inheritdoc />
        [Reactive]
        public Dictionary<string, bool> InitializedServices { get; protected set; } = new Dictionary<string, bool>();

        /// <inheritdoc />
        public virtual void Start()
        {
            try
            {
                StartLoadingWatch();
                InitializeConfiguration();
                InitializeServices();
                RegisterServices();
                BuildContainer();

                Status = CoreStatus.Running;

                WriteLog(new Message("Starting",
                    $"{Name} started successfully.",
                    Name,
                    MessageType.Success));

                StopLoadingWatch();
            }
            catch (Exception e)
            {
                StopLoadingWatch();
                
                WriteLog(new Message(
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
            try
            {
                StartLoadingWatch();

                SaveConfiguration();

                foreach (var service in Services) service.Dispose();

                WriteLog(new Message(
                    "Stopping",
                    $"{Name} stopped successfully.", 
                    Name,
                    MessageType.Success));
                
                StopLoadingWatch();

                WriteLog("----------------------------------------------------");
            }
            catch (Exception e)
            {
                StopLoadingWatch();
                
                WriteLog(new Message(
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
                WriteLog(new Message("Saving configuration",
                    "Error configuration saving:\r\n" + e,
                    Name,
                    MessageType.Error));
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
                WriteLog(new Message("Getting instance",
                    "Error getting instance:\r\n" + e,
                    Name,
                    MessageType.Error));

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

                if (instance.GetType().GetInterfaces().Contains(typeof(IService)))
                {
                    var service = (IService) instance;
                    RegisterService(service);
                    Services.Add(service);

                    return;
                }

                ContainerService.RegisterInstance(instance);
            }
            catch (Exception e)
            {
                WriteLog(new Message("Register instance",
                    "Error registering instance:\r\n" + e,
                    Name,
                    MessageType.Error));
            }
        }

        /// <inheritdoc />
        public virtual void WriteLog(string text)
        {
#if DEBUG
            Console.WriteLine(text);
#endif

            var message = new Message(
                string.Empty,
                text,
                Name,
                MessageType.Information);

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
        public virtual void WriteLog(IMessage message)
        {
            var status = string.Empty;
            var statusColor = new Color();

            switch (message.Type)
            {
                case MessageType.Information:
                    status = "INFO";
                    statusColor = Color.LightGray;
                    break;
                case MessageType.Warning:
                    status = "WARN";
                    statusColor = Color.LightGoldenrodYellow;
                    break;
                case MessageType.Error:
                    status = "ERROR";
                    statusColor = Color.OrangeRed;
                    break;
                case MessageType.Fatal:
                    status = "FATAL";
                    statusColor = Color.DarkRed;
                    break;
                case MessageType.Success:
                    status = "SUCCESS";
                    statusColor = Color.SeaGreen;
                    break;
                case MessageType.Debug:
                    status = "DEBUG";
                    statusColor = Color.SandyBrown;
                    break;
            }

#if DEBUG
            Console.WriteLine(
                "[{0}] [{1}]\t{2}: {3}",
                message.DateTime
                    .ToString(CultureInfo.CurrentCulture)
                    .Pastel(_consoleDataTimeColor),
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
                statusColor = Color.DarkRed;
            }
            else
            {
                status = "ERROR";
                statusColor = Color.OrangeRed;
            }

            var message = new Message(exception, false);

#if DEBUG
            Console.WriteLine(
                "[{0}] [{1}]\t{2}: {3}",
                message.DateTime
                    .ToString(CultureInfo.CurrentCulture)
                    .Pastel(_consoleDataTimeColor),
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
                    ? Json.ReadFile<Configuration>(fileName)
                    : new Configuration();

                Configuration.Initialize();

                Configuration.MessageReceived += OnMessageReceived;

                WriteLog(
                    new Message(
                    "Configuration initialization",
                    $"{Name} configuration initialized successfully.",
                    Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                WriteLog(
                    new Message(
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

                foreach (var obj in ServiceLoader.Objects) Services.Add(obj);

                WriteLog(
                    new Message(
                    "Initializing services",
                    "Services initialized successfully.",
                    "Core",
                    MessageType.Success));

                WriteLog(
                    new Message(
                    "Initializing services",
                    "Number of services were loaded: " + ServiceLoader.Objects.Count(),
                    Name,
                    MessageType.Information));
            }
            catch (Exception e)
            {
                WriteLog(
                    new Message(
                    "Initializing services",
                    "Error occured while initializing services:\r\n" + e,
                    Name,
                    MessageType.Error));
            }
        }

        /// <summary>
        ///     Register services.
        /// </summary>
        private void RegisterServices()
        {
            try
            {
                var collection = new List<IService>(Services);

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
                    new Message(
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
        private void RegisterService(IService service)
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

                    if (!innerInterfaces.Contains(typeof(IService)))
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

                WriteLog(new Message(
                    "Registering service",
                    $"Service {service.Name} ({service.Id}) has been registered successfully.",
                    service.Name,
                    MessageType.Success));

                InitializedServices.Add(service.Name, service.IsInitialized);
            }
            catch (Exception e)
            {
                WriteLog(
                    new Message(
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
        private void OnObjectMessageReceived(object sender, IMessage message)
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
            if (Status == CoreStatus.Starting ||
                Status == CoreStatus.Stopping)
            {
                return;
            }
            
            _loadingWatch.Start();
            
            if (Status == CoreStatus.NotRunning ||
                Status == CoreStatus.Stopped ||
                Status == CoreStatus.Failed)
            {
                WriteLog(new Message("Starting",
                    $"{Name} is starting...",
                    Name,
                    MessageType.Success));
            }

            if (Status == CoreStatus.Running)
            {
                _loadingWatch.Start();

                WriteLog(new Message("Stopping",
                    $"{Name} is stopping...",
                    Name,
                    MessageType.Success));
            }
        }

        /// <summary>
        ///     Stops loading watch.
        /// </summary>
        private void StopLoadingWatch()
        {
            if (Status == CoreStatus.Starting ||
                Status == CoreStatus.Stopping)
            {
                return;
            }
            
            _loadingWatch.Stop();
            
            if (Status == CoreStatus.Stopped)
            {
                WriteLog(new Message($"{Name} stopped",
                    $"Time taken to stop: {Math.Round(_loadingWatch.Elapsed.TotalSeconds, 1)} seconds.",
                    Name,
                    MessageType.Information));
            }
            
            if (Status == CoreStatus.Failed)
            {
                WriteLog(new Message($"{Name} failed to run / stop",
                    $"Time elapsed: {Math.Round(_loadingWatch.Elapsed.TotalSeconds, 1)} seconds.",
                    Name,
                    MessageType.Information));
            }

            if (Status == CoreStatus.Running)
            {
                _loadingWatch.Start();

                WriteLog(new Message($"{Name} started",
                    $"Time taken to start: {Math.Round(_loadingWatch.Elapsed.TotalSeconds, 1)} seconds.",
                    Name,
                    MessageType.Information));
            }
        }
    }
}