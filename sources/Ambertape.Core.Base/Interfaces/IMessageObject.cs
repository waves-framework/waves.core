using System;
using Ambertape.Core.Base.Enums;

namespace Ambertape.Core.Base.Interfaces
{
    /// <summary>
    /// Interface for message object.
    /// </summary>
    public interface IMessageObject
    {
        /// <summary>
        /// Gets ID of message object.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        ///     Gets title of the message object.
        /// </summary>
        string Title { get; }

        /// <summary>
        ///     Gets sender of this message object.
        /// </summary>
        string Sender { get; }

        /// <summary>
        ///     Gets datetime of the message object.
        /// </summary>
        DateTime DateTime { get; }

        /// <summary>
        ///     Gets type of this message object.
        /// </summary>
        MessageType Type { get; }
    }
}