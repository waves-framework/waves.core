using System;
using System.Collections.Generic;
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

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        public sealed override string Name { get; set; }

        /// <inheritdoc />
        public ICollection<IProperty> Properties { get; private set; } = new List<IProperty>();

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
            var configuration = new Configuration();

            foreach (var property in Properties)
                configuration.Properties.Add((Property<dynamic>) property.Clone());

            return configuration;
        }
    }
}