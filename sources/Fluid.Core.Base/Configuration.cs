using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    ///     Configuration base class.
    /// </summary>
    [Serializable]
    public class Configuration : Object, IConfiguration
    {
        /// <summary>
        ///     Creates new instance of configuration.
        /// </summary>
        public Configuration()
        {
        }

        /// <summary>
        ///     Creates new instance of configuration.
        /// </summary>
        /// <param name="name">Name.</param>
        public Configuration(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     Creates new instance of configuration.
        /// </summary>
        /// <param name="id">Id.</param>
        private Configuration(Guid id)
        {
            Id = id;
        }

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public sealed override string Name { get; set; }

        /// <inheritdoc />
        public ICollection<IProperty> Properties { get; private set; } = new ObservableCollection<IProperty>();

        /// <inheritdoc />
        public void AddProperty<T>(string name, T value, bool isReadOnly)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("When adding a property, invalid input was specified!");

            if (!typeof(T).IsSerializable)
                throw new Exception("The specified property does not support serialization " + "(" + name + ").");

            foreach (var p in Properties)
                if (p.Name == name)
                    throw new Exception("A property with the same name already exists (" + name + ").");

            Properties.Add(new Property<T>(name, value, isReadOnly));
        }

        /// <inheritdoc />
        public object GetPropertyValue(string name)
        {
            foreach (var property in Properties)
            {
                if (property.Name != name) continue;
                return property.GetValue();
            }

            throw new Exception("Properties with the same name do not exist " + "(" + name + ").");
        }

        /// <inheritdoc />
        public void SetPropertyValue(string name, object value)
        {
            foreach (var property in Properties)
            {
                if (property.IsReadOnly) continue;
                if (property.Name != name) continue;

                property.SetValue(value);
                return;
            }

            throw new Exception("Properties with the same name do not exist " + "(" + name + ").");
        }

        /// <inheritdoc />
        public void RemoveProperty(string name)
        {
            foreach (var property in Properties)
            {
                if (property.Name != name) continue;
                Properties.Remove(property);
                return;
            }

            throw new Exception("Properties with the same name do not exist " + "(" + name + ").");
        }

        /// <inheritdoc />
        public bool Contains(string name)
        {
            foreach (var property in Properties)
                if (property.Name.Equals(name))
                    return true;

            return false;
        }

        /// <inheritdoc />
        public object Clone()
        {
            var configuration = new Configuration(Id);

            foreach (var property in Properties)
                configuration.Properties.Add((IProperty) property.Clone());

            return configuration;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            var configuration = obj as Configuration;
            if (configuration == null) return Equals(obj);

            var hash1 = this.GetHashCode();
            var hash2 = configuration.GetHashCode();

            return Equals(hash1, hash2);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();

                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);

                foreach (var property in Properties)
                    hashCode = hashCode * 31 + property.GetHashCode();

                return hashCode;
            }
        }

        /// <summary>
        /// Compares two configurations.
        /// </summary>
        /// <param name="other">Other configuration.</param>
        /// <returns>Whether two configurations are equals.</returns>
        protected bool Equals(Configuration other)
        {
            return Id.Equals(other.Id) && Name == other.Name && Equals(Properties, other.Properties);
        }


    }
}