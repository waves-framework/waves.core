using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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
        private readonly ConcurrentDictionary<string, IWavesProperty> _properties;

        /// <summary>
        /// Creates new instance of <see cref="WavesConfiguration"/>.
        /// </summary>
        public WavesConfiguration()
        {
            _properties = new ConcurrentDictionary<string, IWavesProperty>();
        }

        /// <inheritdoc />
        public ICollection<IWavesProperty> GetProperties()
        {
            return _properties.Select(property => property.Value).ToList();
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

            foreach (var p in _properties)
            {
                if (p.Key == property.Name)
                {
                    throw new Exception($"A property with the same name already exists ({property.Name}).");
                }
            }

            var added = _properties.TryAdd(property.Name, property);
            if (!added)
            {
                throw new Exception($"Can't add property to the configuration ({property.Name}).");
            }
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

            if (_properties.Any(p => p.Key == name))
            {
                throw new Exception($"A property with the same name already exists ({name}).");
            }

            var property = new WavesProperty<T>(
                name,
                value);

            var added = _properties.TryAdd(property.Name, property);
            if (!added)
            {
                throw new Exception($"Can't add property to the configuration ({property.Name}).");
            }
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
            genericMethod?.Invoke(this, new[] { name, value });
        }

        /// <inheritdoc />
        public object GetPropertyValue(
            string name)
        {
            foreach (var property in _properties)
            {
                if (property.Key != name)
                {
                    continue;
                }

                return property.Value.GetValue();
            }

            throw new Exception($"Property with the same name not found ({name})!");
        }

        /// <inheritdoc />
        public void SetPropertyValue(
            string name,
            object value)
        {
            foreach (var property in _properties)
            {
                if (property.Key != name)
                {
                    continue;
                }

                var newProperty = (IWavesProperty)property.Value.Clone();

                _properties.TryUpdate(property.Key, newProperty, property.Value);

                return;
            }

            throw new Exception($"Property with the same name not found ({name})!");
        }

        /// <inheritdoc />
        public void RemoveProperty(
            string name)
        {
            foreach (var property in _properties)
            {
                if (property.Key != name)
                {
                    continue;
                }

                var removed = _properties.TryRemove(property.Key, out var removedProperty);
                if (!removed)
                {
                    throw new Exception($"Can't remove property from the configuration ({property.Key}).");
                }

                return;
            }

            throw new Exception($"Property with the same name not found ({name})!");
        }

        /// <inheritdoc />
        public void RewriteConfiguration(
            IWavesConfiguration configuration)
        {
            _properties.Clear();

            foreach (var property in configuration.GetProperties())
            {
                var added = _properties.TryAdd(property.Name, property);
                if (!added)
                {
                    throw new Exception($"Can't add property to the configuration ({property.Name}).");
                }
            }
        }

        /// <inheritdoc />
        public bool Contains(
            string name)
        {
            return _properties.Any(property => property.Key.Equals(name));
        }

        /// <inheritdoc />
        public int GetPropertiesCount()
        {
            return _properties.Count;
        }

        /// <inheritdoc />
        public object Clone()
        {
            var configuration = new WavesConfiguration();

            foreach (var property in _properties)
            {
                configuration.AddProperty((IWavesProperty)property.Value.Clone());
            }

            return configuration;
        }

        /// <inheritdoc />
        public override bool Equals(
            object obj)
        {
            if (obj is not WavesConfiguration configuration)
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
            return _properties != null ? _properties.GetHashCode() : 0;
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
                _properties,
                other._properties);
        }
    }
}
