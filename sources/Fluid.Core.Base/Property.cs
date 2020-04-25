using System;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    ///     Property base class.
    /// </summary>
    [Serializable]
    public class Property<T>: Object, IProperty
    {
        /// <summary>
        ///     Creates new instance of property.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="isReadOnly">Is property read only.</param>
        /// <param name="canBeDeleted">Can a property be deleted.</param>
        public Property(string name, T value, bool isReadOnly, bool canBeDeleted = false)
        {
            IsReadOnly = isReadOnly;
            Name = name;
            Value = value;
            CanBeDeleted = canBeDeleted;
        }

        /// <summary>
        ///     Gets or sets value.
        /// </summary>
        public T Value { get; set; }

        /// <inheritdoc />
        public sealed override string Name { get; set; }

        /// <summary>
        /// Gets or sets whether property is read only.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <inheritdoc />
        public bool CanBeDeleted { get; private set; }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public object GetValue()
        {
            return Value;
        }

        /// <inheritdoc />
        public void SetValue(object value)
        {
            Value = (T) value;
        }

        /// <summary>
        /// Creates new property.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="isReadOnly">Is property read only.</param>
        /// <param name="canBeDeleted">Can a property be deleted.</param>
        /// <returns>Property.</returns>
        public Property<T> Create(string name, T value, bool isReadOnly, bool canBeDeleted = false)
        {
            return new Property<T>(name, value, isReadOnly, canBeDeleted);
        }

        /// <summary>
        ///     Clones property.
        /// </summary>
        /// <returns>Property.</returns>
        public object Clone()
        {
            return new Property<T>(Name, Value, IsReadOnly);
        }
    }
}