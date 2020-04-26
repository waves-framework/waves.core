using System;
using Fluid.Core.Base.Interfaces;
using PropertyChanged;

namespace Fluid.Core.Base
{
    /// <summary>
    ///     Objects base class.
    /// </summary>
    public abstract class Object : ObservableObject, IObject
    {
        /// <inheritdoc />
        public abstract Guid Id { get; }

        /// <inheritdoc />
        public abstract string Name { get; set; }

        /// <inheritdoc />
        public event EventHandler<IMessage> MessageReceived;

        /// <inheritdoc />
        public abstract void Dispose();

        /// <summary>
        ///     Notifies when message received.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Message.</param>
        protected virtual void OnMessageReceived(object sender, IMessage e)
        {
            MessageReceived?.Invoke(sender, e);
        }
    }
}