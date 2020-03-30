using System;
using System.Collections.Generic;
using Fluid.Core.Base.Interfaces;

namespace Fluid.Core.Base
{
    /// <summary>
    /// Configuration base class.
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
        public void AddProperty(IProperty property)
        {
            if (string.IsNullOrEmpty(property.Name) ||
                property.Value == null)
                throw new Exception("When adding a property, invalid input was specified!");

            if (property.Value.GetType().IsSerializable)
            {
                // Проверяем есть ли свойство с таким же именем
                foreach (var p in Properties)
                    if (p.Name == property.Name)
                        throw new Exception("A property with the same name already exists (" + property.Name + ").");

                Properties.Add(property);
            }
            else
            {
                throw new Exception("The specified property does not support serialization " + "(" + property.Name + ").");
            }
        }

        /// <inheritdoc />
        public object GetPropertyValue(string name)
        {
            foreach (var property in Properties)
            {
                if (property.Name != name) continue;
                return property.Value;
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

                property.Value = value;
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
                configuration.Properties.Add((Property) property.Clone());

            return configuration;
        }
    }
}