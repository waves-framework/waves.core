using System;

namespace Fluid.Core.Base.Interfaces
{
    /// <summary>
    /// Interface of property classes.
    /// </summary>
    public interface IProperty : IObject, ICloneable
    {
        /// <summary>
        ///     Gets is property read only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        ///     Gets or sets value of property.
        /// </summary>
        object Value { get; set; }
    }
}