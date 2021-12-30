using System;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    /// Interface for specific message with exception.
    /// </summary>
    public interface IWavesExceptionMessage : IWavesTextMessage
    {
        /// <summary>
        ///     Gets exception.
        /// </summary>
        Exception Exception { get; }
    }
}
