using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Attributes;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Configuration base class.
    /// </summary>
    [JsonObject]
    [WavesObject]
    public class WavesConfiguration :
        WavesObject,
        IWavesConfiguration
    {
        /// <summary>
        ///     Gets collection of properties.
        /// </summary>
        [Reactive]
        [WavesProperty]
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
        public ICollection<IWavesProperty> Properties { get; protected set; } = new List<IWavesProperty>();

        /// <inheritdoc />
        public ICollection<IWavesProperty> GetProperties()
        {
            return Properties;
        }

        /// <inheritdoc />
        public void AddProperty(
            IWavesProperty property)
        {
            if (string.IsNullOrEmpty(property.Name))
            {
                throw new Exception("When adding a property, invalid input was specified!");
            }

            if (!property.GetValue().GetType().IsSerializable)
            {
                throw new Exception(
                    $"The specified property does not support serialization ({property.GetValue().GetType()}).");
            }

            foreach (var p in Properties)
            {
                if (p.Name == property.Name)
                {
                    throw new Exception($"A property with the same name already exists ({property.Name}).");
                }
            }

            Properties.Add(property);
        }

        /// <inheritdoc />
        public void AddProperty<T>(
            string name,
            T value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("When adding a property, invalid input was specified!");
            }

            if (!typeof(T).IsSerializable)
            {
                throw new Exception($"The specified property does not support serialization ({typeof(T)}).");
            }

            foreach (var p in Properties)
            {
                if (p.Name == name)
                {
                    throw new Exception($"A property with the same name already exists ({name}).");
                }
            }

            var property = new WavesProperty<T>(
                name,
                value);

            Properties.Add(property);
        }

        /// <inheritdoc />
        public void AddProperty(
            string name,
            object value)
        {
            if (value == null)
            {
                return;
            }

            var type = value.GetType();
            var method = GetType().GetMethods().Where(x => x.Name.Equals("AddProperty")).FirstOrDefault(x => x.IsGenericMethod);
            var genericMethod = method?.MakeGenericMethod(type);
            genericMethod?.Invoke(this, new object[] { name, value });
        }

        /// <inheritdoc />
        public object GetPropertyValue(
            string name)
        {
            foreach (var property in Properties)
            {
                if (property.Name != name)
                {
                    continue;
                }

                return property.GetValue();
            }

            throw new Exception($"Property with the same name not found ({name})!");
        }

        /// <inheritdoc />
        public void SetPropertyValue(
            string name,
            object value)
        {
            foreach (var property in Properties)
            {
                if (property.Name != name)
                {
                    continue;
                }

                property.SetValue(value);
                return;
            }

            throw new Exception($"Property with the same name not found ({name})!");
        }

        /// <inheritdoc />
        public void RemoveProperty(
            string name)
        {
            foreach (var property in Properties)
            {
                if (property.Name != name)
                {
                    continue;
                }

                Properties.Remove(property);
                return;
            }

            throw new Exception($"Property with the same name not found ({name})!");
        }

        /// <inheritdoc />
        public void RewriteConfiguration(
            IWavesConfiguration configuration)
        {
            Properties.Clear();

            foreach (var property in configuration.GetProperties())
            {
                Properties.Add(property);
            }
        }

        /// <inheritdoc />
        public bool Contains(
            string name)
        {
            foreach (var property in Properties)
            {
                if (property.Name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public int GetPropertiesCount()
        {
            return Properties.Count;
        }

        /// <inheritdoc />
        public object Clone()
        {
            var configuration = new WavesConfiguration();

            foreach (var property in Properties)
            {
                configuration.Properties.Add((IWavesProperty)property.Clone());
            }

            return configuration;
        }

        /// <inheritdoc />
        public override bool Equals(
            object obj)
        {
            if (!(obj is WavesConfiguration configuration))
            {
                return false;
            }

            var hash1 = GetHashCode();
            var hash2 = configuration.GetHashCode();

            return Equals(
                hash1,
                hash2);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Properties != null ? Properties.GetHashCode() : 0;
        }

        /// <summary>
        ///     Compare two configurations.
        /// </summary>
        /// <param name="other">Other configuration.</param>
        /// <returns>Equals or not.</returns>
        protected bool Equals(
            WavesConfiguration other)
        {
            return Equals(
                Properties,
                other.Properties);
        }
    }
}
