using System;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of property classes.
    /// </summary>
    public interface IProperty : IObject, ICloneable
    {
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