using System;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    ///     Base message structure.
    /// </summary>
    public struct Message : IMessage
    {
        /// <summary>
        ///     Creates new instance of Message.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="type">Message type.</param>
        public Message(string title, string text, string sender, MessageType type)
        {
            Title = title;
            Text = text;
            Type = type;
            Sender = sender;
            DateTime = DateTime.Now;
            Exception = null;
        }

        /// <summary>
        ///     Creates new instance of Message.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="isFatal">Is it a fatal error message?</param>
        public Message(Exception exception, bool isFatal)
        {
            Title = "An exception was received";
            Text = exception.Message + ":\r\n" + exception;
            Type = isFatal ? MessageType.Fatal : MessageType.Error;
            Sender = exception.Source;
            DateTime = DateTime.Now;
            Exception = exception;
        }


        /// <inheritdoc />
        public string Title { get; }

        /// <inheritdoc />
        public DateTime DateTime { get; }

        /// <inheritdoc />
        public string Text { get; }

        /// <inheritdoc />
        public string Sender { get; }

        /// <inheritdoc />
        public Exception Exception { get; }

        /// <inheritdoc />
        public MessageType Type { get; }
    }
}