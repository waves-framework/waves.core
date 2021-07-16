using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface for message group.
    /// </summary>
    public interface IWavesMessageGroup : IWavesMessageObject
    {
        /// <summary>
        ///     Gets collection of messages.
        /// </summary>
        ICollection<IWavesMessageObject> Messages { get; }
    }
}
