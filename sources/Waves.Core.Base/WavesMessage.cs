using System;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Base message structure.
    /// </summary>
    public class WavesMessage : IWavesMessage
    {
        /// <summary>
        ///     Creates new instance of <see cref="WavesMessage" />.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="type">Message type.</param>
        public WavesMessage(string title, string text, string sender, WavesMessageType type)
        {
            Title = title;
            Text = text;
            Type = type;
            Sender = sender;
            DateTime = DateTime.Now;
            Exception = null;

            Sender ??= "Unknown";
        }

        /// <summary>
        ///     Creates new instance of <see cref="WavesMessage" />.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="type">Message type.</param>
        public WavesMessage(string title, string text, IWavesObject sender, WavesMessageType type)
        {
            Title = title;
            Text = text;
            Type = type;
            Sender = sender.Name;
            DateTime = DateTime.Now;
            Exception = null;
        }

        /// <summary>
        ///     Creates new instance of <see cref="WavesMessage" />.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <param name="isFatal">Whether error is fatal.</param>
        public WavesMessage(Exception exception, bool isFatal)
        {
            Title = "An exception was received";
            Text = exception.Message + ":\r\n" + exception;
            Type = isFatal ? WavesMessageType.Fatal : WavesMessageType.Error;
            Sender = exception.Source;
            DateTime = DateTime.Now;
            Exception = exception;
        }

        /// <summary>
        ///     Creates new instance of <see cref="WavesMessage" />.
        /// </summary>
        /// <param name="title">Title.</param>
        /// <param name="text">Text.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="exception">Exception.</param>
        /// <param name="isFatal">Whether error is fatal.</param>
        public WavesMessage(string title, string text, string sender, Exception exception, bool isFatal)
        {
            Title = title;
            Text = text;
            Type = isFatal ? WavesMessageType.Fatal : WavesMessageType.Error;
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
        public WavesMessageType Type { get; internal set; }
    }
}