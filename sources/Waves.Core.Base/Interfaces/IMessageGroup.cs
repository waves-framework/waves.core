using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface for message group.
    /// </summary>
    public interface IMessageGroup : IMessageObject
    {
        /// <summary>
        ///     Gets collection of messages.
        /// </summary>
        ICollection<IMessage> Messages { get; }
    }
}