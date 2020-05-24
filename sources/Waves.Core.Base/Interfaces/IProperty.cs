using System;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of property classes.
    /// </summary>
    public interface IProperty : IObject, ICloneable
    {
        /// <summary>
        ///     Gets is property read only.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        ///     Gets whether property can be deleted.
        /// </summary>
        bool CanBeDeleted { get; }

        ///// <summary>
        /////     Gets or sets value of property.
        ///// </summary>
        //object Value { get; set; }

        /// <summary>
        ///     Gets value of property.
        /// </summary>
        object GetValue();

        /// <summary>
        ///     Sets value of property.
        /// </summary>
        /// <param name="value">Value.</param>
        void SetValue(object value);
    }
}