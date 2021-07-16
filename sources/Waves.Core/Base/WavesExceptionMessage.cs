using System;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Specific message object with exception.
    /// </summary>
    public class WavesExceptionMessage : WavesTextMessage,
        IWavesExceptionMessage
    {
        /// <summary>
        ///     Creates new instance of <see cref="WavesExceptionMessage" />.
        /// </summary>
        /// <param name="title">Message title..</param>
        /// <param name="text">Message text.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="type">Message type.</param>
        /// <param name="exception">Exception.</param>
        /// <param name="isFatal">Is exception fatal.</param>
        public WavesExceptionMessage(
            IWavesObject sender,
            Exception exception,
            bool isFatal = false,
            string title = "Exception",
            string text = "An exception was received",
            WavesMessageType type = WavesMessageType.Error)
            : base(text, title, sender, type)
        {
            Type = isFatal ? WavesMessageType.Fatal : WavesMessageType.Error;
            Text = exception.ToString();
            Sender = sender;
            DateTime = DateTime.Now;
            Exception = exception;
        }

        /// <inheritdoc />
        public Exception Exception { get; protected set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Title} - {Text}\r\n{Exception}";
        }
    }
}
