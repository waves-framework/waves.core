using System;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of property classes.
    /// </summary>
    public interface IWavesProperty : IWavesObject,
        ICloneable
    {
        /// <summary>
        /// Gets property ID.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets or sets property name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets value of property.
        /// </summary>
        /// <returns>Returns value.</returns>
        object GetValue();

        /// <summary>
        ///     Sets value of property.
        /// </summary>
        /// <param name="value">Value.</param>
        void SetValue(
            object value);
    }
}
