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
            Value = value;
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
            Value = (T)value;
        }

        /// <summary>
        ///     Creates new instance of property.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        private WavesProperty(
            Guid id,
            string name,
            T value)
        {
            Id = id;
            Name = name;
            Value = value;
        }

        /// <summary>
        ///     Creates new instance of property.
        /// </summary>
        [JsonConstructor]
        private WavesProperty()
        {
        }

        /// <summary>
        ///     Gets value.
        /// </summary>
        [Reactive]
        [JsonProperty]
        public T Value { get; private set; }

        /// <inheritdoc />
        [Reactive]
        [JsonProperty]
        public string Name { get; protected set; }

        /// <inheritdoc />
        [JsonProperty]
        public Guid Id { get; protected set; } = Guid.NewGuid();

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

        /// <inheritdoc />
        public void SetValue(
            object value)
        {
            Value = (T)value;
        }

        /// <summary>
        ///     Clones property.
        /// </summary>
        /// <returns>Property.</returns>
        public object Clone()
        {
            return new WavesProperty<T>(
                Id,
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

                hashCode = (hashCode * 397) ^ Id.GetHashCode();
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
                   Name == other.Name &&
                   Id.Equals(other.Id);
        }
    }
}
