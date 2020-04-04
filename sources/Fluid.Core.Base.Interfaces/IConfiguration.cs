using System;
using System.Collections.Generic;

namespace Fluid.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of configuration classes.
    /// </summary>
    public interface IConfiguration : IObject, ICloneable
    {
        /// <summary>
        ///     Gets properties collection.
        /// </summary>
        ICollection<IProperty> Properties { get; }

        /// <summary>
        /// Adds new property.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="name">Property name.</param>
        /// <param name="value">Value.</param>
        /// <param name="isReadOnly">Whether is property read only.</param>
        void AddProperty<T>(string name, T value, bool isReadOnly);

        /// <summary>
        ///     Gets property value.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <returns>Value.</returns>
        object GetPropertyValue(string name);

        /// <summary>
        ///     Sets property value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetPropertyValue(string name, object value);

        /// <summary>
        ///     Removes property by name.
        /// </summary>
        /// <param name="name">Property name.</param>
        void RemoveProperty(string name);

        /// <summary>
        ///     Gets whether the configuration contains a parameter.
        /// </summary>
        /// <param name="name">Property name.</param>
        bool Contains(string name);
    }
}