﻿using System;
using System.Collections.Generic;

namespace Waves.Core.Base.Interfaces
{
    /// <summary>
    ///     Interface of configuration classes.
    /// </summary>
    public interface IWavesConfiguration : IWavesObject, ICloneable
    {
        /// <summary>
        /// Gets whether configuration is initialized.
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        ///     Initializes configuration.
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Returns collection of properties.
        /// </summary>
        /// <returns></returns>
        ICollection<IWavesProperty> GetProperties();

        /// <summary>
        ///     Adds new property.
        /// </summary>
        /// <param name="property">Property.</param>
        void AddProperty(IWavesProperty property);

        /// <summary>
        ///     Adds new property.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="name">Property name.</param>
        /// <param name="value">Value.</param>
        /// <param name="isReadOnly">Whether is property read only.</param>
        /// <param name="canBeDeleted">Whether property can be deleted.</param>
        void AddProperty<T>(string name, T value, bool isReadOnly, bool canBeDeleted = true);

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
        ///     Rewrites configuration.
        /// </summary>
        /// <param name="configuration">New configuration.</param>
        void RewriteConfiguration(IWavesConfiguration configuration);

        /// <summary>
        ///     Gets whether the configuration contains a parameter.
        /// </summary>
        /// <param name="name">Property name.</param>
        bool Contains(string name);
    }
}