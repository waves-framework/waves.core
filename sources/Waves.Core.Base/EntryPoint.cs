using System;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
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
        [Reactive]
        public bool IsProperty { get; internal set; }

        /// <inheritdoc />
        [Reactive]
        public override string Name { get; set; }

        /// <inheritdoc />
        [Reactive]
        public object Value
        {
            get => _value;
            set
            {
                if (Equals(value, _value)) return;
                _value = value;

                this.RaiseAndSetIfChanged(ref _value, value);

                OnDataReceived(_value);
            }
        }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        [Reactive]
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