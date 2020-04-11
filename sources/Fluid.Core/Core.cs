using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fluid.Core.Base;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.IoC;
using Fluid.Core.Services;
using Fluid.Core.Services.Interfaces;
using Fluid.Utils.Serialization;

namespace Fluid.Core
{
    /// <summary>
    /// Core.
    /// </summary>
    public class Core
    {
        private ILoggingService _loggingService;

        private readonly ServiceManager _serviceManager = new ServiceManager();

        private readonly ICollection<IService> _services = new List<IService>();

        /// <summary>
        /// Event for message receiving handling.
        /// </summary>
        public event EventHandler<IMessage> MessageReceived; 

        /// <summary>
        /// Gets whether Core is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        ///     Gets configuration.
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Gets collections of registered services.
        /// </summary>
        public ICollection<IService> Services => _services;

        /// <summary>
        /// Gets service initialization information dictionary.
        /// Dictionary includes info about base service by default.
        /// </summary>
        public Dictionary<string, bool> CoreInitializationInformationDictionary { get; } = new Dictionary<string, bool>()
        {
            {"Configuration Loader Service", false },
            {"Service Container", false },
            {"Application Loader Service", false },
            {"Keyboard and Mouse Input Service", false },
            {"Logging Service", false },
            {"Module Loader Service", false },
        };

        /// <summary>
        ///     Starts core working.
        /// </summary>
        public virtual void Start()
        {
            try
            {
                InitializeConfiguration();
                InitializeContainer();
                InitializeServices();

                IsRunning = true;

                WriteLogMessage(new Message("Core launching", "Core launching successfully.", "Core",MessageType.Information));
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Core launching", "Error starting kernel:\r\n" + e, "Core", MessageType.Error));
            }
        }

        /// <summary>
        ///     Stops core working.
        /// </summary>
        public virtual void Stop()
        {
            try
            {
                SaveConfiguration();
                StopServices();

                _serviceManager.MessageReceived -= OnServiceMessageReceived;

                WriteLogMessage(new Message("Core stopping", "Core stopping successfully.", "Core",MessageType.Information));
                WriteLog("----------------------------------------------------");
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Core stopping", "Error stopping kernel:\r\n" + e, "Core", MessageType.Error));
            }
        }

        /// <summary>
        ///     Saves configuration.
        /// </summary>
        public void SaveConfiguration()
        {
            try
            {
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
                WriteLogMessage(new Message("Configuration saving", "Error configuration saving:\r\n" + e, "Core", MessageType.Error));
            }
        }

        /// <summary>
        ///     Gets service by type.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>Service.</returns>
        public T GetService<T>()
        {
            try
            {
                return (T) ContainerCore.GetInstance(typeof(T), null);
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Getting service", "Error getting service:\r\n" + e, "Core", MessageType.Error));
                
                return default;
            }
        }

        /// <summary>
        ///     Registers service.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="instance">Instance.</param>
        public void RegisterService<T>(T instance)
        {
            try
            {
                if (!(instance is IService service)) return;

                ContainerCore.RegisterService(instance);

                service.MessageReceived += OnServiceMessageReceived;

                service.Initialize();

                service.LoadConfiguration(Configuration);

                if (service.IsInitialized)
                {
                    _services.Add(service);
                    CoreInitializationInformationDictionary[service.Name] = true;
                }
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Registering service", "Error registering service:\r\n" + e, "Core", MessageType.Error));

                if (!(instance is IService service)) return;

                CoreInitializationInformationDictionary[service.Name] = false;
            }
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

                CoreInitializationInformationDictionary["Configuration Loader Service"] = true;
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Configuration initialization", "Error configuration initialization:\r\n" + e, "Core", MessageType.Error));
            }
        }

        /// <summary>
        /// Initializes container.
        /// </summary>
        private void InitializeContainer()
        {
            try
            {
                ContainerCore.Start();

                CoreInitializationInformationDictionary["Service Container"] = true;
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Container initialization", "Error container initialization:\r\n" + e, "Core", MessageType.Error));

                CoreInitializationInformationDictionary["Service Container"] = false;
            }
        }

        /// <summary>
        ///     Initializes base core services.
        /// </summary>
        private void InitializeServices()
        {
            _serviceManager.MessageReceived += OnServiceMessageReceived;

            _serviceManager.Initialize();

            RegisterService(_serviceManager.GetService<ILoggingService>().First());
            RegisterService(_serviceManager.GetService<IInputService>().First());
            RegisterService(_serviceManager.GetService<IModuleService>().First());
            RegisterService(_serviceManager.GetService<IApplicationService>().First());
        }

        /// <summary>
        /// Stops services.
        /// </summary>
        private void StopServices()
        {
            foreach (var service in _services)
            {
                service.Dispose();
            }
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
        ///     Writes text to log.
        /// </summary>
        /// <param name="text">Text.</param>
        public void WriteLog(string text)
        {
#if DEBUG
            Console.WriteLine(text);
#endif

            if (!CoreInitializationInformationDictionary["Logging Service"]) return;

            OnMessageReceived(new Message(string.Empty, text, string.Empty, MessageType.Information));

            CheckLoggingService();

            _loggingService.WriteTextToLog(text);
        }

        /// <summary>
        ///     Writes message to log.
        /// </summary>
        /// <param name="message">Message..</param>
        public void WriteLogMessage(IMessage message)
        {
#if DEBUG
            Console.WriteLine("{0} {1}: {2}", message.DateTime, message.Sender, message.Title + " - " + message.Text);
#endif

            OnMessageReceived(message);

            if (!CoreInitializationInformationDictionary["Logging Service"]) return;

            CheckLoggingService();

            _loggingService.WriteMessageToLog(message);
        }

        /// <summary>
        ///     Writes exception to log.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="sender">Sender.</param>
        public void WriteLogException(Exception exception, string sender)
        {
#if DEBUG
            Console.WriteLine("Core exception: {0}", exception);
#endif

            OnMessageReceived(new Message(exception, false));

            if (!CoreInitializationInformationDictionary["Logging Service"]) return;

            CheckLoggingService();

            _loggingService.WriteExceptionToLog(exception, sender);
        }

        /// <summary>
        /// Invokes message received event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMessageReceived(IMessage e)
        {
            MessageReceived?.Invoke(this, e);
        }

        private void CheckLoggingService()
        {
            if (_loggingService == null)
                _loggingService = GetService<ILoggingService>();
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