using System;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Base message structure.
    /// </summary>
    public class Message : IMessage
    {
        /// <summary>
        ///     Creates new instance of <see cref="Message" />.
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
        ///     Creates new instance of <see cref="Message" />.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="type">Message type.</param>
        public Message(string title, string text, IObject sender, MessageType type)
        {
            Title = title;
            Text = text;
            Type = type;
            Sender = sender.Name;
            DateTime = DateTime.Now;
            Exception = null;
        }

        /// <summary>
        ///     Creates new instance of <see cref="Message" />.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="isFatal">Whether error is fatal.</param>
        public Message(Exception exception, bool isFatal)
        {
            Title = "An exception was received";
            Text = exception.Message + ":\r\n" + exception;
            Type = isFatal ? MessageType.Fatal : MessageType.Error;
            Sender = exception.Source;
            DateTime = DateTime.Now;
            Exception = exception;
        }

        /// <summary>
        ///     Creates new instance of <see cref="Message" />.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="exception">Exception.</param>
        /// <param name="isFatal">Whether error is fatal.</param>
        public Message(string title, string text, string sender, Exception exception, bool isFatal)
        {
            Title = title;
            Text = text;
            Type = isFatal ? MessageType.Fatal : MessageType.Error;
            Sender = sender;
            DateTime = DateTime.Now;
            Exception = exception;
        }

        /// <inheritdoc />
        public Guid Id { get; internal set; } = Guid.NewGuid();

        /// <inheritdoc />
        [Reactive]
        public string Title { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public DateTime DateTime { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public string Text { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public string Sender { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public Exception Exception { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public MessageType Type { get; internal set; }
    }
}