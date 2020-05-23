using System;
using Ambertape.Core.Base.Enums;

namespace Ambertape.Core.Base.Interfaces
{
    /// <summary>
    ///     Interfaces of Ambertape's message structures.
    /// </summary>
    public interface IMessage : IMessageObject
    {
        /// <summary>
        ///     Gets text of the message.
        /// </summary>
        string Text { get; }

        /// <summary>
        ///     Gets exception.
        /// </summary>
        Exception Exception { get; }
    }
}