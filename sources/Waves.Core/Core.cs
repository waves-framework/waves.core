using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.IoC;
using Waves.Core.Services;
using Waves.Utils.Serialization;

namespace Waves.Core
{
//    public class _Core
//    {
//        private readonly List<IMessage> _pendingMessages = new List<IMessage>();

//        private ILoggingService _loggingService;

//        /// <summary>
//        ///     Gets whether Core is running.
//        /// </summary>
//        public bool IsRunning { get; private set; }

//        /// <summary>
//        ///     Gets configuration.
//        /// </summary>
//        public IConfiguration Configuration { get; private set; }

//        /// <summary>
//        ///     Gets service manager.
//        /// </summary>
//        public Manager ServiceManager { get; } = new Manager();

//        /// <summary>
//        ///     Gets collections of registered services.
//        /// </summary>
//        public ICollection<IService> Services { get; } = new List<IService>();

//        /// <summary>
//        ///     Gets service initialization information dictionary.
//        ///     Dictionary includes info about base service by default.
//        /// </summary>
//        public Dictionary<string, bool> CoreInitializationInformationDictionary { get; } = new Dictionary<string, bool>
//        {
//            {"Configuration Loader Service", false},
//            {"Service Container", false},
//            {"Application Loader Service", false},
//            {"Keyboard and Mouse Input Service", false},
//            {"Logging Service", false},
//            {"Module Loader Service", false}
//        };

//        /// <summary>
//        ///     Event for message receiving handling.
//        /// </summary>
//        public event EventHandler<IMessage> MessageReceived;

//        /// <summary>
//        ///     Starts core working.
//        /// </summary>
//        public virtual void Start()
//        {
//            try
//            {
//                var watch = new Stopwatch();
//                watch.Start();

//                WriteLogMessage(new Message("Core launch", "Core is launching...", "Core",
//                    MessageType.Success));

//                InitializeConfiguration();
//                InitializeContainer();
//                InitializeServices();

//                IsRunning = true;

//                WriteLogMessage(new Message("Core launch", "Core launched successfully.", "Core",
//                    MessageType.Success));

//                watch.Stop();

//                WriteLogMessage(new Message("Core launch",
//                    "Time taken to launch: " + Math.Round(watch.Elapsed.TotalSeconds, 1) + " seconds.", "Core",
//                    MessageType.Success));

//                AddMessageSeparator();
//            }
//            catch (Exception e)
//            {
//                WriteLogMessage(new Message("Core launch", "Error starting kernel.", "Core", e, true));
//            }
//        }

//        /// <summary>
//        ///     Stops core working.
//        /// </summary>
//        public virtual void Stop()
//        {
//            try
//            {
//                SaveConfiguration();
//                StopServices();

//                ServiceManager.MessageReceived -= OnServiceMessageReceived;

//                WriteLogMessage(new Message("Core stop", "Core stopped successfully.", "Core",
//                    MessageType.Success));

//                WriteLog("----------------------------------------------------");
//            }
//            catch (Exception e)
//            {
//                WriteLogMessage(new Message("Core stop", "Error stopping kernel.", "Core", e, true));
//            }
//        }

//        /// <summary>
//        ///     Saves configuration.
//        /// </summary>
//        public void SaveConfiguration()
//        {
//            try
//            {
//                foreach (var service in Services)
//                    try
//                    {
//                        service.SaveConfiguration(Configuration);
//                    }
//                    catch (Exception)
//                    {
//                        throw new Exception("Error saving \"" + service.Name + "\" configuration.");
//                    }

//                CheckConfigurationDirectory();

//                var fileName = Path.Combine(
//                    Environment.CurrentDirectory,
//                    "config",
//                    "core.config");

//                if (File.Exists(fileName))
//                    File.Delete(fileName);

//                Json.WriteToFile(fileName, Configuration);
//            }
//            catch (Exception e)
//            {
//                WriteLogMessage(new Message("Saving configuration", "Error configuration saving:\r\n" + e, "Core",
//                    MessageType.Error));
//            }
//        }

//        /// <summary>
//        ///     Gets service by type.
//        /// </summary>
//        /// <typeparam name="T">Type.</typeparam>
//        /// <returns>Service.</returns>
//        public T GetService<T>()
//        {
//            try
//            {
//                return (T) ContainerCore.GetInstance(typeof(T), null);
//            }
//            catch (Exception e)
//            {
//                WriteLogMessage(new Message("Getting service", "Error getting service.", "Core", e, true));

//                return default;
//            }
//        }

//        /// <summary>
//        ///     Registers service.
//        /// </summary>
//        /// <typeparam name="T">Type.</typeparam>
//        /// <param name="instance">Instance.</param>
//        public void RegisterService<T>(T instance)
//        {
//            try
//            {
//                if (!(instance is IService service)) return;

//                ContainerCore.RegisterService(instance);

//                service.MessageReceived += OnServiceMessageReceived;

//                service.Initialize(this);

//                service.LoadConfiguration(Configuration);

//                if (CoreInitializationInformationDictionary.ContainsKey(service.Name))
//                    CoreInitializationInformationDictionary[service.Name] = service.IsInitialized;
//                else
//                    CoreInitializationInformationDictionary.Add(service.Name, service.IsInitialized);

//                if (service.IsInitialized)
//                    Services.Add(service);
//            }
//            catch (Exception e)
//            {
//                WriteLogMessage(new Message("Registering service", "Error registering service.", "Core", e, true));

//                if (!(instance is IService service)) return;

//                CoreInitializationInformationDictionary[service.Name] = false;
//            }
//        }

//        /// <summary>
//        ///     Writes text to log.
//        /// </summary>
//        /// <param name="text">Text.</param>
//        public virtual void WriteLog(string text)
//        {
//#if DEBUG
//            Console.WriteLine(text);
//#endif

//            if (!CoreInitializationInformationDictionary["Logging Service"]) return;

//            OnMessageReceived(new Message(string.Empty, text, string.Empty, MessageType.Information));

//            if (CheckLoggingService())
//                _loggingService.WriteTextToLog(text);
//        }

//        /// <summary>
//        ///     Writes message to log.
//        /// </summary>
//        /// <param name="message">Message..</param>
//        public virtual void WriteLogMessage(IMessage message)
//        {
//            var status = string.Empty;

//            switch (message.Type)
//            {
//                case MessageType.Information:
//                    status = "INFO";
//                    break;
//                case MessageType.Warning:
//                    status = "WARN";
//                    break;
//                case MessageType.Error:
//                    status = "ERROR";
//                    break;
//                case MessageType.Fatal:
//                    status = "FATAL";
//                    break;
//                case MessageType.Success:
//                    status = "OK";
//                    break;
//                case MessageType.Debug:
//                    status = "DEBUG";
//                    break;
//            }

//#if DEBUG
//            Console.WriteLine("[{0}] [{1}]\t{2}: {3}", message.DateTime, status, message.Sender,
//                message.Title + " - " + message.Text);

//            if (message.Exception != null) Console.WriteLine(message.Exception.ToString());
//#endif

//            OnMessageReceived(message);

//            if (!CoreInitializationInformationDictionary["Logging Service"])
//            {
//                _pendingMessages.Add(message);

//                return;
//            }

//            if (CheckLoggingService())
//                _loggingService.WriteMessageToLog(message);
//        }

//        /// <summary>
//        ///     Writes exception to log.
//        /// </summary>
//        /// <param name="exception">Exception.</param>
//        /// <param name="sender">Sender.</param>
//        /// <param name="isFatal">Sets whether exception is fatal.</param>
//        public virtual void WriteLogException(Exception exception, string sender, bool isFatal)
//        {
//#if DEBUG
//            Console.WriteLine("Core exception: {0}", exception);
//#endif

//            var message = new Message(exception, false);

//            OnMessageReceived(message);

//            if (!CoreInitializationInformationDictionary["Logging Service"])
//            {
//                _pendingMessages.Add(message);

//                return;
//            }

//            if (CheckLoggingService())
//                _loggingService.WriteExceptionToLog(exception, sender, isFatal);
//        }

//        /// <summary>
//        ///     Adds message separator.
//        /// </summary>
//        public virtual void AddMessageSeparator()
//        {
//            if (CheckLoggingService())
//            {
//#if DEBUG
//                Console.WriteLine("-----------------------------------------");
//#endif

//                _loggingService.LastMessages.Add(new MessageSeparator());
//            }
//        }

//        /// <summary>
//        ///     Initializes core configuration.
//        /// </summary>
//        private void InitializeConfiguration()
//        {
//            try
//            {
//                CheckConfigurationDirectory();

//                var fileName = Path.Combine(
//                    Environment.CurrentDirectory,
//                    "config",
//                    "core.config");

//                Configuration = File.Exists(fileName)
//                    ? Json.ReadFile<Configuration>(fileName)
//                    : new Configuration();

//                Configuration.Initialize();

//                CoreInitializationInformationDictionary["Configuration Loader Service"] = true;
//            }
//            catch (Exception e)
//            {
//                WriteLogMessage(new Message("Configuration initialization",
//                    "Error configuration initialization", "Core", e, true));
//            }
//        }

//        /// <summary>
//        ///     Initializes container.
//        /// </summary>
//        private void InitializeContainer()
//        {
//            try
//            {
//                ContainerCore.Start();

//                CoreInitializationInformationDictionary["Service Container"] = true;
//            }
//            catch (Exception e)
//            {
//                WriteLogMessage(new Message("Container initialization", "Error container initialization.", "Core", e,
//                    true));

//                CoreInitializationInformationDictionary["Service Container"] = false;
//            }
//        }

//        /// <summary>
//        ///     Initializes base core services.
//        /// </summary>
//        private void InitializeServices()
//        {
//            ServiceManager.MessageReceived += OnServiceMessageReceived;

//            ServiceManager.Initialize();

//            var method = typeof(Core).GetMethod("RegisterService");
//            if (method == null) return;

//            foreach (var service in ServiceManager.Services)
//                try
//                {
//                    var type = service.GetType();
//                    var interfaces = type.GetInterfaces().ToList();

//                    Type result = null;

//                    foreach (var i in interfaces)
//                    {
//                        var innerInterfaces = i.GetInterfaces();

//                        if (!innerInterfaces.Contains(typeof(IService))) continue;
//                        result = i;
//                        break;
//                    }

//                    if (result == null) return;

//                    var genericMethod = method.MakeGenericMethod(result);
//                    genericMethod.Invoke(this, new object[] {service});

//                    WriteLogMessage(new Message("Registering", "Service has been registered successfully.",
//                        service.Name,
//                        MessageType.Success));
//                }
//                catch (Exception e)
//                {
//                    WriteLogMessage(new Message("Registering",
//                        "Error registering service.", service.Name, e, true));
//                }
//        }

//        /// <summary>
//        ///     Stops services.
//        /// </summary>
//        private void StopServices()
//        {
//            foreach (var service in Services) service.Dispose();
//        }

//        /// <summary>
//        ///     Notifies when service receive message.
//        /// </summary>
//        /// <param name="sender">Sender.</param>
//        /// <param name="message">Message.</param>
//        private void OnServiceMessageReceived(object sender, IMessage message)
//        {
//            WriteLogMessage(message);
//        }

//        /// <summary>
//        ///     Invokes message received event.
//        /// </summary>
//        /// <param name="e"></param>
//        protected virtual void OnMessageReceived(IMessage e)
//        {
//            MessageReceived?.Invoke(this, e);
//        }

//        private bool CheckLoggingService()
//        {
//            if (_loggingService != null) return true;

//            _loggingService = GetService<ILoggingService>();

//            if (_loggingService == null) return false;

//            foreach (var message in _pendingMessages) _loggingService.WriteMessageToLog(message);

//            _pendingMessages.Clear();

//            return true;
//        }

//        /// <summary>
//        ///     Checks configuration directory.
//        /// </summary>
//        private void CheckConfigurationDirectory()
//        {
//            var directoryName = Path.Combine(
//                Environment.CurrentDirectory,
//                "config");

//            if (!Directory.Exists(directoryName))
//                Directory.CreateDirectory(directoryName);
//        }
//    }

    /// <summary>
    ///     Core.
    /// </summary>
    public class Core : ICore
    {
        private readonly List<IMessage> _pendingMessages = new List<IMessage>();

        private ILoggingService _loggingService;

        private ICollection<IService> _services = new List<IService>();

        /// <inheritdoc />
        public event EventHandler<IMessage> MessageReceived;

        /// <summary>
        ///     Gets service manager.
        /// </summary>
        public Manager ServiceManager { get; } = new Manager();

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

                WriteLogMessage(new Message("Core launch", "Core is launching...", "Core",
                    MessageType.Success));

                InitializeConfiguration();
                InitializeContainer();
                InitializeServices();

                IsRunning = true;

                WriteLogMessage(new Message("Core launch", "Core launched successfully.", "Core",
                    MessageType.Success));

                watch.Stop();

                WriteLogMessage(new Message("Core launch",
                    "Time taken to launch: " + Math.Round(watch.Elapsed.TotalSeconds, 1) + " seconds.", "Core",
                    MessageType.Success));

                WriteLogSeparator();
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Core launch", "Error starting kernel.", "Core", e, true));
            }
        }

        /// <inheritdoc />
        public void StartAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual void Stop()
        {
            try
            {
                SaveConfiguration();
                StopServices();

                ServiceManager.MessageReceived -= OnServiceMessageReceived;

                WriteLogMessage(new Message("Core stop", "Core stopped successfully.", "Core",
                    MessageType.Success));

                WriteLog("----------------------------------------------------");
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Core stop", "Error stopping kernel.", "Core", e, true));
            }
        }

        /// <inheritdoc />
        public void StopAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SaveConfiguration()
        {
            try
            {
                foreach (var service in Services)
                    try
                    {
                        service.SaveConfiguration(Configuration);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Error saving \"" + service.Name + "\" configuration.");
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
        public Task SaveConfigurationAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public T GetService<T>()
        {
            try
            {
                return (T) ContainerCore.GetInstance(typeof(T), null);
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Getting service", "Error getting service.", "Core", e, true));

                return default;
            }
        }

        /// <inheritdoc />
        public Task<T> GetServiceAsync<T>()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void RegisterService<T>(T instance)
        {
            try
            {
                if (!(instance is IService service)) return;

                ContainerCore.RegisterService(instance);

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
        public Task RegisterServiceAsync<T>(T instance)
        {
            throw new NotImplementedException();
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
        public Task WriteLogAsync(string text)
        {
            throw new NotImplementedException();
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
            Console.WriteLine("[{0}] [{1}]\t{2}: {3}", message.DateTime, status, message.Sender,
                message.Title + " - " + message.Text);

            if (message.Exception != null) Console.WriteLine(message.Exception.ToString());
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
        public Task WriteLogMessageAsync(IMessage message)
        {
            throw new NotImplementedException();
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
        public Task WriteLogExceptionAsync(Exception exception, string sender, bool isFatal)
        {
            throw new NotImplementedException();
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

        /// <inheritdoc />
        public Task WriteLogSeparatorAsync()
        {
            throw new NotImplementedException();
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
                ContainerCore.Start();

                InitializedServices["Service Container"] = true;
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
            ServiceManager.MessageReceived += OnServiceMessageReceived;

            ServiceManager.Initialize();

            var method = typeof(Core).GetMethod("RegisterService");
            if (method == null) return;

            foreach (var service in ServiceManager.Services)
            {
                try
                {
                    var type = service.GetType();
                    var interfaces = type.GetInterfaces().ToList();

                    Type result = null;

                    foreach (var i in interfaces)
                    {
                        var innerInterfaces = i.GetInterfaces();

                        if (!innerInterfaces.Contains(typeof(IService))) continue;
                        result = i;
                        break;
                    }

                    if (result == null) return;

                    var genericMethod = method.MakeGenericMethod(result);
                    genericMethod.Invoke(this, new object[] {service});

                    WriteLogMessage(new Message("Registering", "Service has been registered successfully.",
                        service.Name,
                        MessageType.Success));
                }
                catch (Exception e)
                {
                    WriteLogMessage(new Message("Registering",
                        "Error registering service.", service.Name, e, true));
                }
            }
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