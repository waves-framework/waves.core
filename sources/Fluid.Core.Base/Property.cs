using System;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    ///     Property base class.
    /// </summary>
    [Serializable]
    public class Property<T> : Object, IProperty
    {
        /// <summary>
        ///     Creates new instance of property.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="isReadOnly">Is property read only.</param>
        public Property(string name, T value, bool isReadOnly)
        {
            IsReadOnly = isReadOnly;
            Name = name;
            Value = value;
        }

        /// <inheritdoc />
        public sealed override string Name { get; set; }

        /// <inheritdoc />
        public bool IsReadOnly { get; private set; }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets value.
        /// </summary>
        public T Value { get; private set; }

        /// <inheritdoc />
        public object GetValue()
        {
            return Value;
        }

        /// <inheritdoc />
        public void SetValue(object value)
        {
            Value = (T)value;
        }

        /// <summary>
        /// Clones property.
        /// </summary>
        /// <returns>Property.</returns>
        public object Clone()
        {
            return new Property<T>(Name, Value, IsReadOnly);
        }
    }
}