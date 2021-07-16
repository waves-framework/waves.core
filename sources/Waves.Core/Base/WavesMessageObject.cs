using System;
using Waves.Core.Base.Enums;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    /// Message object abstraction.
    /// </summary>
    public abstract class WavesMessageObject : WavesObject, IWavesMessageObject
    {
        /// <inheritdoc />
        public Guid Id { get; protected set; } = Guid.NewGuid();

        /// <inheritdoc />
        public string Title { get; protected set; }

        /// <inheritdoc />
        public IWavesObject Sender { get; protected set; }

        /// <inheritdoc />
        public DateTime DateTime { get; protected set; }

        /// <inheritdoc />
        public WavesMessageType Type { get; protected set; }
    }
}
