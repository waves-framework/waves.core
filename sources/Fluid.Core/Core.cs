using System;
using System.Collections.Generic;
using System.IO;
using Fluid.Core.Base;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.IoC;
using Fluid.Core.Logging;
using Fluid.Core.Logging.Events;
using Fluid.Core.Logging.Sinks.Console;
using Fluid.Core.Logging.Sinks.File;
using Fluid.Core.Services;
using Fluid.Core.Services.Interfaces;
using Fluid.Utils.Serialization;

namespace Fluid.Core
{
    public static class Core
    {
        /// <summary>
        /// Инициализировано ли логирование.
        /// </summary>
        public static bool IsLogInitialized { get; private set; } = false;

        /// <summary>
        /// Инициализирована ли конфигурация.
        /// </summary>
        public static bool IsConfigurationInitialized { get; private set; } = false;

        /// <summary>
        /// Инициализированы ли сервисы.
        /// </summary>
        public static bool IsServicesInitialized { get; private set; }

        /// <summary>
        ///     Конфигурация ядра.
        /// </summary>
        public static IConfiguration Configuration { get; private set; }

        /// <summary>
        ///     Коллекция загруженных сервисов.
        /// </summary>
        public static ICollection<IService> Services { get; } = new List<IService>();

        /// <summary>
        ///     Запуск ядра.
        /// </summary>
        public static void Start()
        {
            try
            {
                WriteLog("--------------------------------------------");
                WriteLogMessage(new Message("Запуск ядра", "Инициализация запуска ядра...", "Core", MessageType.Information));

                ContainerCore.Start();

                InitializeLog();
                InitializeConfiguration();
                InitializeServices();

                if (IsLogInitialized && IsConfigurationInitialized && IsServicesInitialized)
                    WriteLogMessage(new Message("Запуск ядра", "Ядро успешно запущено.", "Core", MessageType.Information));
                else
                    WriteLogMessage(new Message("Запуск ядра", "Ядро запущено, но не все компоненты инициализированы.", "Core", MessageType.Information));
            }
            catch (Exception e)
            {
                WriteLogMessage(new Message("Запуск ядра", "Ошибка запуска ядра:\r\n" + e, "Core", MessageType.Error));
                throw;
            }
        }

        /// <summary>
        ///     Остановка ядра.
        /// </summary>
        public static void Stop()
        {
            try
            {
                SaveConfiguration();

                WriteLogMessage(new Message("Остановка ядра", "Инициализация остановки ядра...", "Core", MessageType.Information));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Сохраняет конфигурацию.
        /// </summary>
        public static void SaveConfiguration()
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
                OnServiceMessageReceived("Core", new Message("Ошибка", "При записи файла конфигурации возникла ошибка:\r\n" + e, "Core", MessageType.Error));
            }
        }

        /// <summary>
        ///     Получение сервиса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            try
            {
                return (T)ContainerCore.GetInstance(typeof(T), null);
            }
            catch (Exception e)
            {
                OnServiceMessageReceived("Core", new Message("Ошибка", "При получении сервиса " + typeof(T) + " возникла ошибка:\r\n" + e, "Core", MessageType.Error));
                return default;
            }
        }

        /// <summary>
        ///     Регистрация сервиса
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void RegisterService<T>(T instance)
        {
            try
            {
                if (!(instance is IService service)) return;

                ContainerCore.RegisterService(instance);

                service.MessageReceived += OnServiceMessageReceived;

                service.Initialize();

                service.LoadConfiguration(Configuration);

                if (service.IsInitialized)
                    Services.Add(service);
            }
            catch (Exception e)
            {
                OnServiceMessageReceived("Core", new Message("Ошибка", "При регистрации сервиса" + instance + " возникла ошибка:\r\n" + e, "Core", MessageType.Error));
            }
        }

        /// <summary>
        ///     Инициализация логирования.
        /// </summary>
        private static void InitializeLog()
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("logs\\log.txt", LogEventLevel.Debug, "{Message:lj}{NewLine}")
                    .CreateLogger();

                IsLogInitialized = true;
            }
            catch (Exception e)
            {
                OnServiceMessageReceived("Core", new Message("Ошибка", "При инициализации логов возникла ошибка:\r\n" + e, "Core", MessageType.Error));
            }
        }

        /// <summary>
        ///     Инициализация конфигурации ядра.
        /// </summary>
        private static void InitializeConfiguration()
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

                IsConfigurationInitialized = true;
            }
            catch (Exception e)
            {
                OnServiceMessageReceived("Core", new Message("Ошибка", "При инициализации конфигурации возникла ошибка:\r\n" + e, "Core", MessageType.Error));
            }
        }

        /// <summary>
        ///     Инициализация сервисов ядра.
        /// </summary>
        private static void InitializeServices()
        {
            var moduleService = new ModuleService();

            RegisterService<IModuleService>(moduleService);
            RegisterService<IApplicationService>(new ApplicationService());
            RegisterService<IAdcService>(new AdcService(moduleService));
            RegisterService<IInputService>(new InputService());
            RegisterService<ISensorService>(new SensorService());

            IsServicesInitialized = true;
        }

        /// <summary>
        ///     Уведомление о приеме сообщения от сервиса.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private static void OnServiceMessageReceived(object sender, IMessage message)
        {
            WriteLogMessage(message);
        }

        /// <summary>
        /// Запись строки в лог.
        /// </summary>
        /// <param name="text"></param>
        public static void WriteLog(string text)
        {
            Log.Information(text);
        }

        /// <summary>
        ///     Записывает сообщение в лог.
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLogMessage(IMessage message)
        {
            switch (message.Type)
            {
                case MessageType.Information:
                    Log.Information("{0} [{1}]: {2} - {3}",
                        message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                        message.Title, message.Sender, message.Text);
                    break;
                case MessageType.Warning:
                    Log.Warning("{0} [{1}]: {2} - {3}",
                        message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                        message.Title, message.Sender, message.Text);
                    break;
                case MessageType.Error:
                    Log.Error("{0} [{1}]: {2} - {3}",
                        message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                        message.Title, message.Sender, message.Text);
                    break;
                case MessageType.Success:
                    Log.Information("{0} [{1}]: {2} - {3}",
                        message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                        message.Title, message.Sender, message.Text);
                    break;
                case MessageType.Debug:
                    Log.Debug("{0} [{1}]: {2} - {3}",
                        message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                        message.Title, message.Sender, message.Text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Проверка директории.
        /// </summary>
        private static void CheckConfigurationDirectory()
        {
            var directoryName = Path.Combine(
                Environment.CurrentDirectory,
                "config");

            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
        }
    }
}