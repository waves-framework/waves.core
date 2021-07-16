using System;
using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of configuration classes.
    /// </summary>
    public interface IWavesConfiguration : IWavesObject,
        ICloneable
    {
        /// <summary>
        ///     Gets collection of properties.
        /// </summary>
        /// <returns>Returns collection of properties.</returns>
        ICollection<IWavesProperty> GetProperties();

        /// <summary>
        ///     Adds new property.
        /// </summary>
        /// <param name="property">Property.</param>
        void AddProperty(
            IWavesProperty property);

        /// <summary>
        ///     Adds new property.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="name">Property name.</param>
        /// <param name="value">Value.</param>
        void AddProperty<T>(
            string name,
            T value);

        /// <summary>
        ///     Adds new property.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Value.</param>
        void AddProperty(
            string name,
            object value);

        /// <summary>
        ///     Gets property value.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <returns>Value.</returns>
        object GetPropertyValue(
            string name);

        /// <summary>
        ///     Sets property value.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <param name="value">Value.</param>
        void SetPropertyValue(
            string name,
            object value);

        /// <summary>
        ///     Removes property by name.
        /// </summary>
        /// <param name="name">Property name.</param>
        void RemoveProperty(
            string name);

        /// <summary>
        ///     Rewrites configuration.
        /// </summary>
        /// <param name="configuration">New configuration.</param>
        void RewriteConfiguration(
            IWavesConfiguration configuration);

        /// <summary>
        ///     Gets whether the configuration contains a parameter.
        /// </summary>
        /// <param name="name">Property name.</param>
        /// <returns>Returns whether configuration contains property or not.</returns>
        bool Contains(
            string name);

        /// <summary>
        /// Gets count of registered properties.
        /// </summary>
        /// <returns>Count of registered properties.</returns>
        int GetPropertiesCount();
    }
}
