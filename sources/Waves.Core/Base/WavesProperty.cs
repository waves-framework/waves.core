using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    /// Property base.
    /// </summary>
    /// <typeparam name="T">Property type.</typeparam>
    [JsonObject]
    public class WavesProperty<T> :
        WavesObject,
        IWavesProperty
    {
        private T _value;

        /// <summary>
        ///     Creates new instance of property.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public WavesProperty(
            string name,
            T value)
        {
            Name = name;
            _value = value;
        }

        /// <summary>
        /// Creates new instance of property.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public WavesProperty(
            string name,
            object value)
        {
            Name = name;
            _value = (T)value;
        }

        /// <summary>
        ///     Gets value.
        /// </summary>
        public T Value => _value;

        /// <inheritdoc />
        public string Name { get; }

        /// <summary>
        /// Creates new instance of <see cref="WavesProperty{T}"/>.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <returns>Instance of <see cref="WavesProperty{T}"/>.</returns>
        public static WavesProperty<T> Create(
            string name,
            object value)
        {
            return new WavesProperty<T>(name, (T)value);
        }

        /// <inheritdoc />
        public object GetValue()
        {
            return Value;
        }

        /// <summary>
        ///     Clones property.
        /// </summary>
        /// <returns>Property.</returns>
        public object Clone()
        {
            return new WavesProperty<T>(
                Name,
                Value);
        }

        /// <inheritdoc />
        public override bool Equals(
            object obj)
        {
            return obj is WavesProperty<T> property && Equals(property);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T>.Default.GetHashCode(Value);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }

        /// <summary>
        ///     Compares two properties.
        /// </summary>
        /// <param name="other">Other property.</param>
        /// <returns>Property.</returns>
        protected bool Equals(
            WavesProperty<T> other)
        {
            return EqualityComparer<T>.Default.Equals(
                       Value,
                       other.Value) &&
                   Name == other.Name;
        }
    }
}
