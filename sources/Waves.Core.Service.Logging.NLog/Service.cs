using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;
using NLog;
using NLog.Common;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Core.Service.Logging.NLog
{
    /// <summary>
    ///     Logging service.
    /// </summary>
    [Export(typeof(IWavesService))]
    public class LoggingService : WavesService, ILoggingService
    {
        private Logger _logger;

        /// <inheritdoc />
        public override Guid Id => Guid.Parse("D17B3463-C126-4023-B22F-1A031636A343");

        /// <inheritdoc />
        public override string Name { get; set; } = "Logging Service (NLog)";

        /// <inheritdoc />
        [Reactive]
        public int LastMessagesCount { get; private set; } = 250;

        /// <inheritdoc />
        [Reactive]
        public ICollection<IWavesMessageObject> LastMessages { get; set; } 
            = new ObservableCollection<IWavesMessageObject>();

        /// <inheritdoc />
        public override void Initialize(IWavesCore core)
        {
            if (IsInitialized) return;

            Core = core;

            try
            {
                _logger = LogManager.GetCurrentClassLogger();

                InternalLogger.LogToConsole = false;

                IsInitialized = true;

                OnMessageReceived(
                    this,
                    new WavesMessage(
                        "Initialization", 
                        "Service has been initialized.", 
                        Name, 
                        WavesMessageType.Information));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new WavesMessage(
                        "Service initialization", 
                        "Error service initialization.", 
                        Name, 
                        e, 
                        false));
            }
        }

        /// <inheritdoc />
        public override void LoadConfiguration()
        {
            try
            {
                LastMessagesCount = LoadConfigurationValue(
                    Core.Configuration, 
                    "LoggingService-LastMessagesCount", 
                    250);

                OnMessageReceived(
                    this, 
                    new WavesMessage(
                        "Loading configuration",
                        "Configuration loaded successfully.", 
                        Name,
                    WavesMessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(
                    this,
                    new WavesMessage(
                        "Loading configuration", 
                        "Error loading configuration.", 
                        Name,
                        e, 
                        false));
            }
        }

        /// <inheritdoc />
        public override void SaveConfiguration()
        {
            try
            {
                Core.Configuration.SetPropertyValue(
                    "LoggingService-LastMessagesCount", 
                    LastMessagesCount);

                OnMessageReceived(
                    this, 
                    new WavesMessage(
                        "Saving configuration",
                        "Configuration saved successfully.", 
                        Name,
                    WavesMessageType.Success));
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new WavesMessage(
                        "Saving configuration", 
                        "Error saving configuration.", 
                        Name, 
                        e, 
                        false));
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
                var message = new WavesMessage(string.Empty, text, string.Empty, WavesMessageType.Information);

                AddMessageToCollection(message);

                _logger.Info("{0} {1}",
                    $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}",
                    text);
            }
            catch (Exception e)
            {
                OnMessageReceived(
                    this,
                    new WavesMessage(
                        "Writing text to log", 
                        "Error writing text to log.", 
                        Name, 
                        e, 
                        false));
            }
        }

        /// <inheritdoc />
        public void WriteMessageToLog(IWavesMessage message)
        {
            if (!IsInitialized) return;

            try
            {
                AddMessageToCollection(message);

                switch (message.Type)
                {
                    case WavesMessageType.Information:
                        _logger.Info(
                            "{0} {1} {2}: {3} - {4}",
                            $"{message.DateTime.ToShortDateString()} {message.DateTime.ToShortTimeString()}",
                            "[INFORMATION]",
                            message.Sender,
                            message.Title, 
                            message.Text);
                        break;
                    case WavesMessageType.Warning:
                        _logger.Warn(
                            "{0} {1} {2}: {3} - {4}",
                            $"{message.DateTime.ToShortDateString()} {message.DateTime.ToShortTimeString()}",
                            "[WARNING]",
                            message.Sender, 
                            message.Title, 
                            message.Text);
                        break;
                    case WavesMessageType.Error:
                        _logger.Error(
                            "{0} {1} {2}: {3} - {4}",
                            $"{message.DateTime.ToShortDateString()} {message.DateTime.ToShortTimeString()}",
                            "[ERROR]",
                            message.Sender, 
                            message.Title, 
                            message.Text);
                        break;
                    case WavesMessageType.Success:
                        _logger.Info(
                            "{0} {1} {2}: {3} - {4}",
                            $"{message.DateTime.ToShortDateString()} {message.DateTime.ToShortTimeString()}",
                            "[SUCCESS]",
                            message.Sender, 
                            message.Title, 
                            message.Text);
                        break;
                    case WavesMessageType.Debug:
                        _logger.Debug(
                            "{0} {1} {2}: {3} - {4}",
                            $"{message.DateTime.ToShortDateString()} {message.DateTime.ToShortTimeString()}",
                            "[DEBUG]",
                            message.Sender, 
                            message.Title,
                            message.Text);
                        break;
                    case WavesMessageType.Fatal:
                        _logger.Fatal("{0} {1} {2}: {3} - {4}",
                            $"{message.DateTime.ToShortDateString()} {message.DateTime.ToShortTimeString()}",
                            "[FATAL]",
                            message.Sender,
                            message.Title, 
                            message.Text);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(
                    this,
                    new WavesMessage(
                        "Writing message to log", 
                        "Error writing message to log.", 
                        Name, 
                        e, 
                        false));
            }
        }

        /// <inheritdoc />
        public void WriteExceptionToLog(Exception exception, string sender, bool isFatal)
        {
            if (!IsInitialized) return;

            try
            {
                var message = new WavesMessage(exception.Source, exception.Message, sender, WavesMessageType.Error);

                AddMessageToCollection(message);

                if (isFatal)
                    _logger.Fatal("{0} {1} {2}: {3} - {4}",
                        $"{message.DateTime.ToShortDateString()} {message.DateTime.ToShortTimeString()}",
                        "[FATAL]",
                        message.Sender, 
                        message.Title, 
                        message.Text);
                else
                    _logger.Error("{0} {1} {2}: {3} - {4}",
                        $"{message.DateTime.ToShortDateString()} {message.DateTime.ToShortTimeString()}",
                        "[ERROR]",
                        message.Sender, 
                        message.Title, 
                        message.Text);
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new WavesMessage("Writing exception to log", "Error writing exception to log.", Name, e, false));
            }
        }

        /// <summary>
        ///     Adds message to collection.
        /// </summary>
        /// <param name="message">Message.</param>
        private void AddMessageToCollection(IWavesMessage message)
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
                        if (lastObject is IWavesMessageGroup group)
                        {
                            group.Messages.Add(message);

                            return;
                        }

                        if (lastObject is IWavesMessage previousMessage)
                        {
                            var g = new WavesMessageGroup(previousMessage.Title, previousMessage.Sender,
                                previousMessage.DateTime, previousMessage.Type);

                            g.Messages.Add(previousMessage);
                            g.Messages.Add(message);

                            LastMessages.Remove(previousMessage);

                            LastMessages.Add(g);
                        }
                    }
                    else
                    {
                        var g = new WavesMessageGroup(message.Title, message.Sender,
                            message.DateTime, message.Type);
                        
                        g.Messages.Add(message);
                        
                        LastMessages.Add(g);
                    }
                }
                else
                {
                    var g = new WavesMessageGroup(message.Title, message.Sender,
                        message.DateTime, message.Type);
                    
                    g.Messages.Add(message);
                        
                    LastMessages.Add(g);
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this,
                    new WavesMessage("Adding message", "Message has not been added to log collection.", Name, e, false));
            }
        }
    }
}