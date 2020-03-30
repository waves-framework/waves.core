using System;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    ///     Property base class.
    /// </summary>
    [Serializable]
    public class Property : Object, IProperty
    {
        /// <summary>
        ///     Creates new instance of property.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="isReadOnly">Is property read only.</param>
        public Property(string name, object value, bool isReadOnly)
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

        /// <inheritdoc />
        public object Value { get; set; }

        /// <inheritdoc />
        public object Clone()
        {
            return new Property(Name, Value, IsReadOnly);
        }
    }
}