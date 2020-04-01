using System;
using System.Collections.Generic;
using Fluid.Core.Base;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;
using Fluid.Core.Services.Interfaces;

namespace Fluid.Core.Services.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class Service : Services.Service, ILoggingService
    {
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
                
            }
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
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void WriteTextToLog(string text)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void WriteMessageToLog(IMessage message)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void WriteExceptionToLog(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}