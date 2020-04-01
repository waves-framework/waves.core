using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for logging services.
    /// </summary>
    public interface ILoggingService : IService
    {
        /// <summary>
        /// Gets number of last messages.
        /// </summary>
        int LastMessagesCount { get; }
        
        /// <summary>
        /// Get log path.
        /// </summary>
        string LogPath { get; }
        
        /// <summary>
        /// Gets collection of last log messages.
        /// </summary>
        ICollection<IMessage> LastMessages { get; }

        /// <summary>
        /// Writes text to log.
        /// </summary>
        /// <param name="text"></param>
        void WriteTextToLog(string text);
        
        /// <summary>
        /// Writes message to log.
        /// </summary>
        /// <param name="message">Message.</param>
        void WriteMessageToLog(IMessage message);

        /// <summary>
        /// Writes exception to log.
        /// </summary>
        /// <param name="exception">Exception.</param>
        void WriteExceptionToLog(Exception exception);
    }
}