using System;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    /// Objects base class.
    /// </summary>
    public abstract class Object : ObservableObject, IObject
    {
        /// <inheritdoc />
        public abstract Guid Id { get; }

        /// <inheritdoc />
        public abstract string Name { get; set; }

        /// <inheritdoc />
        public event EventHandler<IMessage> MessageReceived;
    }
}