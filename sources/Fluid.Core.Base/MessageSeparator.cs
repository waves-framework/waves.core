using System;
using Fluid.Core.Base.Enums;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <inheritdoc />
    public class MessageSeparator : IMessageObject
    {
        /// <inheritdoc />
        public Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public string Title { get; } = string.Empty;

        /// <inheritdoc />
        public string Sender { get; } = string.Empty;

        /// <inheritdoc />
        public DateTime DateTime { get; } = DateTime.Now;

        /// <inheritdoc />
        public MessageType Type { get; } = MessageType.Information
    }
}