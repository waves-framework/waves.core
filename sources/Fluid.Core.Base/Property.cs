using System;
using System.Collections.Generic;
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
        /// <param name="id">Id.</param>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="isReadOnly">Is property read only.</param>
        /// <param name="canBeDeleted">Can a property be deleted.</param>
        private Property(Guid id, string name, T value, bool isReadOnly, bool canBeDeleted = false)
        {
            Id = id;
            IsReadOnly = isReadOnly;
            Name = name;
            Value = value;
            CanBeDeleted = canBeDeleted;
        }

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
        ///     Clones property.
        /// </summary>
        /// <returns>Property.</returns>
        public object Clone()
        {
            return new Property<T>(Id, Name, Value, IsReadOnly, CanBeDeleted);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return Equals((Property<T>)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T>.Default.GetHashCode(Value);

                hashCode = (hashCode * 397) ^ Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsReadOnly.GetHashCode();
                hashCode = (hashCode * 397) ^ CanBeDeleted.GetHashCode();

                return hashCode;
            }
        }

        /// <summary>
        /// Compares two properies.
        /// </summary>
        /// <param name="other">Other property.</param>
        /// <returns>Property.</returns>
        protected bool Equals(Property<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value) && Name == other.Name && IsReadOnly == other.IsReadOnly && CanBeDeleted == other.CanBeDeleted && Id.Equals(other.Id);
        }


    }
}