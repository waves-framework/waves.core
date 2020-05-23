using System;

namespace Ambertape.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of base Ambertape's object.
    /// </summary>
    public interface IObject : IObservableObject, IDisposable
    {
        /// <summary>
        ///     Gets object's ID.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        ///     Gets object's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Event for message receiving handling.
        /// </summary>
        event EventHandler<IMessage> MessageReceived;
    }
}