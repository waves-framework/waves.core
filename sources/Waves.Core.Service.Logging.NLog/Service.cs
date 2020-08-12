using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;
using NLog;
using NLog.Common;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Service.Logging.NLog
{
    /// <summary>
    ///     Logging service.
    /// </summary>
    [Export(typeof(IService))]
    public class LoggingService : Base.Service, ILoggingService
    {
        private readonly string _currentDirectory = Environment.CurrentDirectory;

        private Logger _logger;

        /// <summary>
        ///     Gets instance of Core.
        /// </summary>
        public ICore Core { get; private set; }

        /// <inheritdoc />
        public override Guid Id => Guid.Parse("D17B3463-C126-4023-B22F-1A031636A343");

        /// <inheritdoc />
        public override string Name { get; set; } = "Logging Service (NLog)";

        /// <inheritdoc />
        public int LastMessagesCount { get; private set; } = 250;

        /// <inheritdoc />
        public ICollection<IMessageObject> LastMessages { get; } = new ObservableCollection<IMessageObject>();

        /// <inheritdoc />
        public override void Initialize(ICore core)
        {
            if (IsInitialized) return;

            Core = core;

            try
            {
                _logger = LogManager.GetCurrentClassLogger();

                InternalLogger.LogToConsole = false;

                IsInitialized = true;

                OnMessageReceived(this,
                    new Message("Initialization", "Service has been initialized.", Name, MessageType.Information));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Service initialization", "Error service initialization.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public override void LoadConfiguration(IConfiguration configuration)
        {
            try
            {
                LastMessagesCount = LoadConfigurationValue(configuration, "LoggingService-LastMessagesCount", 250);

                OnMessageReceived(this, new Message("Loading configuration", "Configuration loads successfully.", Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Loading configuration", "Error loading configuration.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public override void SaveConfiguration(IConfiguration configuration)
        {
            try
            {
                configuration.SetPropertyValue("LoggingService-LastMessagesCount", LastMessagesCount);

                OnMessageReceived(this, new Message("Saving configuration", "Configuration saved successfully.", Name,
                    MessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Saving configuration", "Error saving configuration.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public override void Dispose()
        {
        }

        /// <inheritdoc />
        public void WriteTextToLog(string text)
        {
            if (!IsInitialized) return;

            try
            {
                var message = new Message(string.Empty, text, string.Empty, MessageType.Information);

                AddMessageToCollection(message);

                _logger.Info("{0} {1}",
                    DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(),
                    text);
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Writing text to log", "Error writing text to log.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public void WriteMessageToLog(IMessage message)
        {
            if (!IsInitialized) return;

            try
            {
                AddMessageToCollection(message);

                switch (message.Type)
                {
                    case MessageType.Information:
                        _logger.Info("{0} {1} {2}: {3} - {4}",
                            message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                            "[INFORMATION]",
                            message.Sender, message.Title, message.Text);
                        break;
                    case MessageType.Warning:
                        _logger.Warn("{0} {1} {2}: {3} - {4}",
                            message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                            "[WARNING]",
                            message.Sender, message.Title, message.Text);
                        break;
                    case MessageType.Error:
                        _logger.Error("{0} {1} {2}: {3} - {4}",
                            message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                            "[ERROR]",
                            message.Sender, message.Title, message.Text);
                        break;
                    case MessageType.Success:
                        _logger.Info("{0} {1} {2}: {3} - {4}",
                            message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                            "[SUCCESS]",
                            message.Sender, message.Title, message.Text);
                        break;
                    case MessageType.Debug:
                        _logger.Debug("{0} {1} {2}: {3} - {4}",
                            message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                            "[DEBUG]",
                            message.Sender, message.Title, message.Text);
                        break;
                    case MessageType.Fatal:
                        _logger.Fatal("{0} {1} {2}: {3} - {4}",
                            message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                            "[FATAL]",
                            message.Sender, message.Title, message.Text);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Writing message to log", "Error writing message to log.", Name, e, false));
            }
        }

        /// <inheritdoc />
        public void WriteExceptionToLog(Exception exception, string sender, bool isFatal)
        {
            if (!IsInitialized) return;

            try
            {
                var message = new Message(exception.Source, exception.Message, sender, MessageType.Error);

                AddMessageToCollection(message);

                if (isFatal)
                    _logger.Fatal("{0} {1} {2}: {3} - {4}",
                        message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                        "[FATAL]",
                        message.Sender, message.Title, message.Text);
                else
                    _logger.Error("{0} {1} {2}: {3} - {4}",
                        message.DateTime.ToShortDateString() + " " + message.DateTime.ToShortTimeString(),
                        "[ERROR]",
                        message.Sender, message.Title, message.Text);
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Writing exception to log", "Error writing exception to log.", Name, e, false));
            }
        }

        /// <summary>
        ///     Adds message to collection.
        /// </summary>
        /// <param name="message">Message.</param>
        private void AddMessageToCollection(IMessage message)
        {
            try
            {
                if (LastMessages.Count > 1)
                {
                    var lastObject = LastMessages.Last();

                    if (lastObject.Title == message.Title &&
                        lastObject.Type == message.Type &&
                        lastObject.Sender == message.Sender)
                    {
                        if (lastObject is IMessageGroup group)
                        {
                            group.Messages.Add(message);

                            return;
                        }

                        if (lastObject is IMessage previousMessage)
                        {
                            var g = new MessageGroup(previousMessage.Title, previousMessage.Sender,
                                previousMessage.DateTime, previousMessage.Type);

                            g.Messages.Add(previousMessage);
                            g.Messages.Add(message);

                            LastMessages.Remove(previousMessage);

                            LastMessages.Add(g);
                        }
                    }
                    else
                    {
                        LastMessages.Add(message);
                    }
                }
                else
                {
                    LastMessages.Add(message);
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new Message("Adding message", "Message has not been added to log collection.", Name, e, false));
            }
        }
    }
}