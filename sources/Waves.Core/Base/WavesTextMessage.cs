using System;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Base message structure.
    /// </summary>
    public class WavesTextMessage :
        WavesMessageObject,
        IWavesTextMessage
    {
        /// <summary>
        ///     Creates new instance of <see cref="WavesTextMessage" />.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="title">Title.</param>
        /// <param name="sender">Sender.</param>
        /// <param name="type">Message type.</param>
        public WavesTextMessage(
            string text,
            string title = "",
            IWavesObject sender = null,
            WavesMessageType type = WavesMessageType.Information)
        {
            Title = title;
            Text = text;
            Type = type;
            Sender = sender;
            DateTime = DateTime.Now;
        }

        /// <inheritdoc />
        [Reactive]
        public string Text { get; internal set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Title} - {Text}";
        }
    }
}
