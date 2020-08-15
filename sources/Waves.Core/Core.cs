using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;
using Waves.Utils.Serialization;

namespace Waves.Core
{
    /// <summary>
    /// Core.
    /// </summary>
    public class Core : ReactiveObject, ICore
    {
        private readonly List<IMessage> _pendingMessages = new List<IMessage>();

        /// <inheritdoc />
        public event EventHandler<IMessage> MessageReceived;

        /// <inheritdoc />
        [Reactive]
        public bool IsRunning { get; protected set; }

        /// <summary>
        ///     Gets service loader.
        /// </summary>
        public ServiceLoader ServiceLoader { get; } = new ServiceLoader();

        /// <summary>
        /// Gets instance of logging service.
        /// </summary>
        protected ILoggingService LoggingService { get; private set; }

        /// <summary>
        /// Gets instance of container service.
        /// </summary>
        protected IContainerService ContainerService { get; private set; }

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
        public void Start()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                WriteLog(new Message("Core start",
                    "Core is starting...",
                    "Core",
                    MessageType.Success));

                InitializeConfiguration();
                InitializeServices();
                RegisterServices();
                BuildContainer();

                IsRunning = true;

                WriteLog(new Message("Core start", 
                    "Core started successfully.", 
                    "Core",
                    MessageType.Success));

                watch.Stop();

                WriteLog(new Message("Core start",
                    "Time taken to start: " + Math.Round(watch.Elapsed.TotalSeconds, 1) + " seconds.", 
                    "Core",
                    MessageType.Information));
            }
            catch (Exception e)
            {
                WriteLog(new Message(
                    "Core start", 
                    "Error starting core.", 
                    "Core", 
                    e, 
                    true));
            }
        }

        /// <inheritdoc />
        public void Stop()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                SaveConfiguration();

                foreach (var service in Services)
                {
                    service.Dispose();
                }

                WriteLog(new Message("Core stop", "Core stopped successfully.", "Core",
                    MessageType.Success));

                watch.Stop();

                WriteLog(new Message("Core stop",
                    "Time taken to stop: " + Math.Round(watch.Elapsed.TotalSeconds, 1) + " seconds.", "Core",
                    MessageType.Success));

                WriteLog("----------------------------------------------------");
            }
            catch (Exception e)
            {
                WriteLog(new Message(
                    "Core stop", 
                    "Error stopping kernel.", 
                    "Core", 
                    e, 
                    true));
            }
        }

        /// <inheritdoc />
        public void SaveConfiguration()
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
                    "Core",
                    MessageType.Error));
            }
        }

        /// <inheritdoc />
        public T GetInstance<T>() where T : class
        {
            try
            {
                if (ContainerService == null)
                {
                    throw new NullReferenceException("Container service was not registered.");
                }

                return ContainerService.GetInstance<T>();
            }
            catch (Exception e)
            {
                WriteLog(new Message("Getting instance",
                    "Error getting instance:\r\n" + e,
                    "Core",
                    MessageType.Error));

                return null;
            }
        }

        /// <inheritdoc />
        public ICollection<T> GetInstances<T>() where T : class
        {
            try
            {
                if (ContainerService == null)
                {
                    throw new NullReferenceException("Container service was not registered.");
                }

                return ContainerService.GetInstances<T>();
            }
            catch (Exception e)
            {
                WriteLog(new Message("Getting instances",
                    "Error getting instances:\r\n" + e,
                    "Core",
                    MessageType.Error));

                return null;
            }
        }

        /// <inheritdoc />
        public void RegisterInstance<T>(T instance) where T : class
        {
            try
            {
                if (ContainerService == null)
                {
                    throw new NullReferenceException("Container service was not registered.");
                }

                if (instance.GetType().GetInterfaces().Contains(typeof(IService)))
                {
                    var service = (IService)instance;
                    RegisterService(service);
                    Services.Add(service);

                    return;
                }

                ContainerService.RegisterInstance<T>(instance);
            }
            catch (Exception e)
            {
                WriteLog(new Message("Register instance",
                    "Error registering instance:\r\n" + e,
                    "Core",
                    MessageType.Error));
            }
        }

        /// <inheritdoc />
        public void RegisterInstances<T>(ICollection<T> instances) where T : class
        {
            try
            {
                if (ContainerService == null)
                {
                    throw new NullReferenceException("Container service was not registered.");
                }

                foreach (var instance in instances)
                {
                    if (instance.GetType().GetInterfaces().Contains(typeof(IService)))
                    {
                        var service = (IService)instance;
                        RegisterService(service);
                        Services.Add(service);
                    }
                }

                ContainerService.RegisterInstances<T>(instances);
            }
            catch (Exception e)
            {
                WriteLog(new Message("Register instances",
                    "Error registering instances:\r\n" + e,
                    "Core",
                    MessageType.Error));
            }
        }

        /// <inheritdoc />
        public virtual void WriteLog(string text)
        {
#if DEBUG
            Console.WriteLine(text);
#endif

            OnMessageReceived(new Message(string.Empty, text, string.Empty, MessageType.Information));

            CheckLoggingService();

            if (CheckLoggingService())
            {
                LoggingService.WriteTextToLog(text);

                return;
            }

            _pendingMessages.Add(new Message(string.Empty, text, "Core", MessageType.Information));
        }

        /// <inheritdoc />
        public virtual void WriteLog(IMessage message)
        {
            var status = string.Empty;

            switch (message.Type)
            {
                case MessageType.Information:
                    status = "INFO";
                    break;
                case MessageType.Warning:
                    status = "WARN";
                    break;
                case MessageType.Error:
                    status = "ERROR";
                    break;
                case MessageType.Fatal:
                    status = "FATAL";
                    break;
                case MessageType.Success:
                    status = "OK";
                    break;
                case MessageType.Debug:
                    status = "DEBUG";
                    break;
            }

#if DEBUG
            Console.WriteLine(
                "[{0}] [{1}]\t{2}: {3}",
                message.DateTime,
                status,
                message.Sender,
                message.Title + " - " + message.Text);

            if (message.Exception != null)
                Console.WriteLine(message.Exception.ToString());
#endif

            OnMessageReceived(message);

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
#if DEBUG
            Console.WriteLine("Core exception: {0}", exception);
#endif

            var message = new Message(exception, false);

            OnMessageReceived(message);

            if (CheckLoggingService())
            {
                LoggingService.WriteExceptionToLog(exception, sender, isFatal);

                return;
            }

            _pendingMessages.Add(message);
        }

        /// <summary>
        ///     Invokes message received event.
        /// </summary>
        /// <param name="e">Message.</param>
        protected virtual void OnMessageReceived(IMessage e)
        {
            MessageReceived?.Invoke(this, e);
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

                WriteLog(new Message("Configuration initialization",
                    "Core configuration initialized successfully.",
                    "Core",
                    MessageType.Success));
            }
            catch (Exception e)
            {
                WriteLog(new Message("Configuration initialization",
                    "Error configuration initialization", "Core", e, true));
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
                {
                    throw new Exception("Error initializing services: services not initialized.");
                }

                foreach (var obj in ServiceLoader.Objects)
                {
                    Services.Add(obj);   
                }

                WriteLog(new Message(
                    "Initializing services",
                    "Services initialized successfully.",
                    "Core",
                    MessageType.Success));

                WriteLog(new Message(
                    "Initializing services",
                    "Number of services were loaded: " + ServiceLoader.Objects.Count(),
                    "Core",
                    MessageType.Information));
            }
            catch (Exception e)
            {
                WriteLog(new Message(
                    "Initializing services",
                    "Error Initializing services:\r\n" + e,
                    "Core",
                    MessageType.Error));
            }
        }

        /// <summary>
        /// Register services.
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
                        ContainerService = (IContainerService)service;
                        collection.Remove(service);
                        RegisterService(service);
                    }

                    if (service.GetType().GetInterfaces().Contains(typeof(ILoggingService)))
                    {
                        LoggingService = (ILoggingService)service;
                        collection.Remove(service);
                        RegisterService(service);
                        WritePendingMessages();
                    }
                }

                // if container service is null, than throw exception.
                if (ContainerService == null)
                {
                    throw new NullReferenceException("Container service not initialized or initialized with errors.");
                }

                // if logging service is null, than throw exception.
                if (LoggingService == null)
                {
                    throw new NullReferenceException("Logging service not initialized or initialized with errors.");
                }

                // register other services
                foreach (var service in collection)
                {
                    RegisterService(service);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Registers service.
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
                genericMethod?.Invoke(ContainerService, new object[] { service });

                WriteLog(new Message(
                    "Registering service",
                    "Service has been registered successfully.",
                    service.Name,
                    MessageType.Success));

                InitializedServices.Add(service.Name, service.IsInitialized);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        ///     Invokes message received event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Message.</param>
        private void OnMessageReceived(object sender, IMessage e)
        {
            MessageReceived?.Invoke(sender, e);
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
        /// Builds container.
        /// </summary>
        private void BuildContainer()
        {
            ContainerService?.Build();;
        }

        /// <summary>
        /// Check whether logging service is loaded.
        /// </summary>
        /// <returns></returns>
        private bool CheckLoggingService()
        {
            return LoggingService != null;
        }

        /// <summary>
        /// Writes pending log messages to log.
        /// </summary>
        private void WritePendingMessages()
        {
            foreach (var message in _pendingMessages)
                LoggingService.WriteMessageToLog(message);

            _pendingMessages.Clear();
        }
    }
}