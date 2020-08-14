using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Utils.Serialization;

namespace Waves.Core
{
    /// <summary>
    ///     Core.
    /// </summary>
    public class Core : ICore
    {
        private bool _isContainerBuilt;
        
        private readonly List<IMessage> _pendingMessages = new List<IMessage>();

        private MethodInfo _registerServiceMethod;
        
        private ContainerBuilder _builder;

        private IContainer _container;

        private ILoggingService _loggingService;

        private ICollection<IService> _services = new List<IService>();

        /// <inheritdoc />
        public event EventHandler<IMessage> MessageReceived;

        /// <summary>
        ///     Gets service manager.
        /// </summary>
        public ServiceLoader ServiceManager { get; } = new ServiceLoader();

        /// <inheritdoc />
        public bool IsRunning { get; private set; }

        /// <inheritdoc />
        public IConfiguration Configuration { get; private set; }

        /// <inheritdoc />
        public ICollection<IService> Services { get; } = new List<IService>();

        /// <inheritdoc />
        public Dictionary<string, bool> InitializedServices { get; } = new Dictionary<string, bool>();

        /// <inheritdoc />
        public virtual void Start()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                WriteLogMessage(new Message("Core start", "Core is starting...", "Core",
                    MessageType.Success));

                InitializeConfiguration();
                InitializeServices();
                InitializeContainer();

                IsRunning = true;

                WriteLogMessage(new Message("Core start", "Core started successfully.", "Core",
                    MessageType.Success));

                watch.Stop();

                WriteLogMessage(new Message("Core start",
                    "Time taken to start: " + Math.Round(watch.Elapsed.TotalSeconds, 1) + " seconds.", "Core",
                    MessageType.Success));

                WriteLogSeparator();
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Core start", "Error starting core.", "Core", e, true));
            }
        }

        /// <inheritdoc />
        public virtual void Stop()
        {
            try
            {
                var watch = new Stopwatch();
                watch.Start();

                SaveConfiguration();
                StopServices();

                ServiceManager.MessageReceived -= OnServiceMessageReceived;

                WriteLogMessage(new Message("Core stop", "Core stopped successfully.", "Core",
                    MessageType.Success));

                watch.Stop();

                WriteLogMessage(new Message("Core stop",
                    "Time taken to stop: " + Math.Round(watch.Elapsed.TotalSeconds, 1) + " seconds.", "Core",
                    MessageType.Success));

                WriteLog("----------------------------------------------------");
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Core stop", "Error stopping kernel.", "Core", e, true));
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
                        service.SaveConfiguration(Configuration);
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
                WriteLogMessage(new Message("Saving configuration", "Error configuration saving:\r\n" + e, "Core",
                    MessageType.Error));
            }
        }

        /// <inheritdoc />
        public T GetService<T>()
        {
            try
            {
                return _container == null ? default : _container.Resolve<T>();
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Getting service", "Error getting service.", "Core", e, true));

                return default;
            }
        }

        /// <inheritdoc />
        public void RegisterService<T>(T instance) where T : class
        {
            try
            {
                if (!(instance is IService service)) return;

                if (!_isContainerBuilt)
                    _builder.RegisterInstance(instance).As<T>();
                else
                {
                    // ???
                }

                service.MessageReceived += OnServiceMessageReceived;

                service.Initialize(this);
                service.LoadConfiguration(Configuration);

                if (InitializedServices.ContainsKey(service.Name))
                    InitializedServices[service.Name] = service.IsInitialized;
                else
                    InitializedServices.Add(service.Name, service.IsInitialized);

                if (service.IsInitialized)
                    Services.Add(service);
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Registering service", "Error registering service.", "Core", e, true));

                if (!(instance is IService service)) return;

                InitializedServices[service.Name] = false;
            }
        }

        /// <inheritdoc />
        public virtual void WriteLog(string text)
        {
#if DEBUG
            Console.WriteLine(text);
#endif

            OnMessageReceived(new Message(string.Empty, text, string.Empty, MessageType.Information));

            if (CheckLoggingService())
            {
                _loggingService.WriteTextToLog(text);

                return;
            }

            _pendingMessages.Add(new Message(string.Empty, text, "Core", MessageType.Information));
        }

        /// <inheritdoc />
        public virtual void WriteLogMessage(IMessage message)
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
                _loggingService.WriteMessageToLog(message);

                return;
            }

            _pendingMessages.Add(message);
        }

        /// <inheritdoc />
        public virtual void WriteLogException(Exception exception, string sender, bool isFatal)
        {
#if DEBUG
            Console.WriteLine("Core exception: {0}", exception);
#endif

            var message = new Message(exception, false);

            OnMessageReceived(message);

            if (CheckLoggingService())
            {
                _loggingService.WriteExceptionToLog(exception, sender, isFatal);

                return;
            }

            _pendingMessages.Add(message);
        }

        /// <inheritdoc />
        public void WriteLogSeparator()
        {
#if DEBUG
            Console.WriteLine("-----------------------------------------");
#endif
            if (!CheckLoggingService()) return;

            _loggingService.LastMessages.Add(new MessageSeparator());
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

                InitializedServices["Configuration Loader Service"] = true;
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Configuration initialization",
                    "Error configuration initialization", "Core", e, true));
            }
        }

        /// <summary>
        ///     Initializes container.
        /// </summary>
        private void InitializeContainer()
        {
            try
            {
                _container = _builder.Build();

                _isContainerBuilt = true;
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Container initialization", "Error container initialization.", "Core", e,
                    true));

                InitializedServices["Service Container"] = false;
            }
        }

        /// <summary>
        ///     Initializes base core services.
        /// </summary>
        private void InitializeServices()
        {
            _builder = new ContainerBuilder();

            ServiceManager.Initialize(this);

            if (ServiceManager.Objects == null)
            {
                throw new Exception("Error initializing services.");
            }

            _registerServiceMethod = typeof(Core).GetMethod("RegisterService");
            if (_registerServiceMethod == null) return;
            
            foreach (var service in ServiceManager.Objects)
            {
                try
                {
                    InitializeService(service);
                }
                catch (Exception e)
                {
                    WriteLogMessage(new Message("Registering",
                        "Error registering service.", service.Name, e, true));
                }
            }
        }

        /// <summary>
        /// Initializes service.
        /// </summary>
        /// <param name="service">Instance of service.</param>
        private void InitializeService(IService service)
        {
            if (_registerServiceMethod == null) return;
            
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

            var genericMethod = _registerServiceMethod.MakeGenericMethod(result);
            genericMethod.Invoke(this, new object[] {service});

            WriteLogMessage(new Message("Registering", "Service has been registered successfully.",
                service.Name,
                MessageType.Success));
        }

        /// <summary>
        ///     Stops services.
        /// </summary>
        private void StopServices()
        {
            foreach (var service in Services) service.Dispose();
        }

        /// <summary>
        ///     Notifies when service receive message.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="message">Message.</param>
        private void OnServiceMessageReceived(object sender, IMessage message)
        {
            WriteLogMessage(message);
        }

        /// <summary>
        ///     Invokes message received event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMessageReceived(IMessage e)
        {
            MessageReceived?.Invoke(this, e);
        }

        private bool CheckLoggingService()
        {
            if (_loggingService != null) return true;

            _loggingService = GetService<ILoggingService>();

            if (_loggingService == null) return false;

            foreach (var message in _pendingMessages) _loggingService.WriteMessageToLog(message);

            _pendingMessages.Clear();

            return true;
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
    }
}