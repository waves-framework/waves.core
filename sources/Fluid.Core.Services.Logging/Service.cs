using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;
using Fluid.Core.Base;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.Services.Interfaces;
using Serilog;
using Serilog.Events;

namespace Fluid.Core.Services.Logging
{
    /// <summary>
    /// Logging service.
    /// </summary>
    [Export(typeof(IService))]
    public class Service : Services.Service, ILoggingService
    {
        private readonly string _currentDirectory = Environment.CurrentDirectory;
        
        /// <summary>
        /// Gets default log path.
        /// </summary>
        private string DefaultLogPath => Path.Combine(_currentDirectory, "logs");
        
        /// <inheritdoc />
        public override Guid Id => Guid.Parse("D17B3463-C126-4023-B22F-1A031636A343");

        /// <inheritdoc />
        public override string Name { get; set; } = "Logging Service";

        /// <inheritdoc />
        public int LastMessagesCount { get; private set; } = 250;

        /// <inheritdoc />
        public string LogPath { get; private set;}

        /// <inheritdoc />
        public ICollection<IMessage> LastMessages { get; private set; } = new List<IMessage>(); 
        
        /// <inheritdoc />
        public override void Initialize()
        {
            if (IsInitialized) return;

            IsInitialized = true;

            OnMessageReceived(this, new Message("Initialization.", "Service was initialized.", Name, MessageType.Information));
        }

        /// <inheritdoc />
        public override void LoadConfiguration(IConfiguration configuration)
        {
            LastMessagesCount = LoadConfigurationValue<int>(configuration, "ModuleService-ModulesPaths");
            LogPath = LoadConfigurationValue<string>(configuration, "ModuleService-LogPath");

            // if null log path.
            if (string.IsNullOrEmpty(LogPath))
            {
                if (!Directory.Exists(DefaultLogPath))
                    Directory.CreateDirectory(DefaultLogPath);

                LogPath = Path.Combine(DefaultLogPath, "log.txt");
            }
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(LogPath, LogEventLevel.Debug, "{Message:lj}{NewLine}")
                .CreateLogger();
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
            configuration.SetPropertyValue("LoggingService-LastMessagesCount", LastMessagesCount);
            configuration.SetPropertyValue("LoggingService-LogPath", LogPath);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            Log.CloseAndFlush();
        }

        /// <inheritdoc />
        public void WriteTextToLog(string text)
        {
            if (!IsInitialized) return;
            
            var message = new Message(string.Empty, text, string.Empty, MessageType.Information);
            
            AddMessageToCollection(message);

            Log.Information("{0} [{1}]: {2}",
                DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(),
                text);
        }

        /// <inheritdoc />
        public void WriteMessageToLog(IMessage message)
        {
            if (!IsInitialized) return;
            
            AddMessageToCollection(message);

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

        /// <inheritdoc />
        public void WriteExceptionToLog(Exception exception, string sender)
        {
            if (!IsInitialized) return;
            
            var message = new Message(exception.Source, exception.Message, sender, MessageType.Information);
            
            AddMessageToCollection(message);

            Log.Error("{0} [{1}]: {2} - {3}",
                DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(),
                "An exception thrown: ", sender, exception.Message + "\r\n" + exception);
        }

        /// <summary>
        /// Adds message to collection.
        /// </summary>
        /// <param name="message">Message.</param>
        private void AddMessageToCollection(IMessage message)
        {
            LastMessages.Add(message);
            
            if (LastMessages.Count > LastMessagesCount)
                (LastMessages as List<IMessage>)?.RemoveAt(0);
        }
    }
}