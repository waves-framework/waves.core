using System;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    ///     Entry point base class.
    /// </summary>
    public class EntryPoint : Object, IEntryPoint
    {
        private object _value = new object();

        /// <summary>
        ///     Creates new instance of entry point.
        /// </summary>
        /// <param name="parent">Parent.</param>
        /// <param name="isProperty">Whether a point is a property.</param>
        public EntryPoint(IModule parent, bool isProperty)
        {
            Parent = parent;
            IsProperty = isProperty;
        }

        /// <inheritdoc />
        public bool IsProperty { get; }

        /// <inheritdoc />
        public override string Name { get; set; }

        /// <inheritdoc />
        public object Value
        {
            get => _value;
            set
            {
                if (Equals(value, _value)) return;
                _value = value;
                OnPropertyChanged();
                OnDataReceived(_value);
            }
        }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public IModule Parent { get; set; }

        /// <inheritdoc />
        [field: NonSerialized]
        public event EventHandler<object> DataReceived;

        /// <inheritdoc />
        public override void Dispose()
        {
            Parent = null;
            Value = null;
        }

        /// <summary>
        ///     Notifies when new data received.
        /// </summary>
        /// <param name="e">Data.</param>
        protected virtual void OnDataReceived(object e)
        {
            DataReceived?.Invoke(this, e);
        }
    }
}