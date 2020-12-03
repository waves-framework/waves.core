using System;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Objects base class.
    /// </summary>
    public abstract class WavesObject : WavesObservableObject, IWavesObject
    {
        /// <inheritdoc />
        public abstract Guid Id { get; }
        
        /// <inheritdoc />
        public WavesColor Color { get; } = WavesColor.Random();

        /// <inheritdoc />
        public abstract string Name { get; set; }

        /// <inheritdoc />
        public event EventHandler<IWavesMessage> MessageReceived;

        /// <inheritdoc />
        public abstract void Dispose();

        /// <summary>
        ///     Notifies when message received.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Message.</param>
        protected void OnMessageReceived(object sender, IWavesMessage e)
        {
            MessageReceived?.Invoke(sender, e);
        }
    }
}