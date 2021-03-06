using System;
using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces.Services
{
    /// <summary>
    ///     Interface for logging services.
    /// </summary>
    public interface ILoggingService : IWavesService
    {
        /// <summary>
        ///     Gets number of last messages.
        /// </summary>
        int LastMessagesCount { get; }

        /// <summary>
        ///     Gets collection of last log messages.
        /// </summary>
        ICollection<IWavesMessageObject> LastMessages { get; }

        /// <summary>
        ///     Writes text to log.
        /// </summary>
        /// <param name="text"></param>
        void WriteTextToLog(string text);

        /// <summary>
        ///     Writes message to log.
        /// </summary>
        /// <param name="message">Message.</param>
        void WriteMessageToLog(IWavesMessage message);

        /// <summary>
        ///     Writes exception to log.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="isFatal">Sets whether exception is fatal.</param>
        void WriteExceptionToLog(Exception exception, string sender, bool isFatal);
    }
}