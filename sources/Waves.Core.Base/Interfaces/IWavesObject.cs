using System;
using ReactiveUI;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of base Waves's object.
    /// </summary>
    public interface IWavesObject : IWavesObservableObject, IDisposable
    {
        /// <summary>
        ///     Gets object's ID.
        /// </summary>
        Guid Id { get; }
        
        /// <summary>
        /// Gets unique color of object.
        /// </summary>
        WavesColor Color { get; }

        /// <summary>
        ///     Gets object's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Event for message receiving handling.
        /// </summary>
        event EventHandler<IWavesMessage> MessageReceived;
    }
}