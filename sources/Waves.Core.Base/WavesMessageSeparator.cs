using System;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <inheritdoc />
    public class WavesMessageSeparator : IWavesMessageObject
    {
        /// <inheritdoc />
        public Guid Id { get; internal set; } = Guid.NewGuid();

        /// <inheritdoc />
        [Reactive]
        public string Title { get; internal set; } = string.Empty;

        /// <inheritdoc />
        [Reactive]
        public string Sender { get; internal set; } = string.Empty;

        /// <inheritdoc />
        [Reactive]
        public DateTime DateTime { get; internal set; } = DateTime.Now;

        /// <inheritdoc />
        [Reactive]
        public WavesMessageType Type { get; internal set; } = WavesMessageType.Information;
    }
}