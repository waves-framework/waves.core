using System;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface for connection classes.
    /// </summary>
    public interface IWavesConnection : IWavesObject, IDisposable, ICloneable
    {
        /// <summary>
        ///     Get input entry point of this connection.
        /// </summary>
        IWavesEntryPoint Input { get; set; }

        /// <summary>
        ///     Gets output entry point of this connection.
        /// </summary>
        IWavesEntryPoint Output { get; set; }
    }
}