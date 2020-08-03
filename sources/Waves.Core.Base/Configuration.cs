using System;
using System.Collections.Generic;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Configuration base class.
    /// </summary>
    [Serializable]
    public class Configuration : Object, IConfiguration
    {
        private bool _isDisposed;

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

        /// <summary>
        ///     Gets collection of properties.
        /// </summary>
        [Reactive]
        public ICollection<IProperty> Properties { get; private set; } = new List<IProperty>();

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        [Reactive]
        public sealed override string Name { get; set; }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (_isDisposed) return;

            foreach (var property in Properties) property.MessageReceived -= OnPropertyMessageReceived;

            _isDisposed = true;
        }

        /// <inheritdoc />
        public void Initialize()
        {
            try
            {
                foreach (var property in Properties) property.MessageReceived += OnPropertyMessageReceived;
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message(e, false));
            }
        }

        /// <inheritdoc />
        public ICollection<IProperty> GetProperties()
        {
            return Properties;
        }

        /// <inheritdoc />
        public void AddProperty(IProperty property)
        {
            try
            {
                if (string.IsNullOrEmpty(property.Name))
                    throw new Exception("When adding a property, invalid input was specified!");

                if (!property.GetValue().GetType().IsSerializable)
                    throw new Exception("The specified property does not support serialization " + "(" +
                                        property.GetValue().GetType() + ").");

                foreach (var p in Properties)
                    if (p.Name == property.Name)
                        throw new Exception("A property with the same name already exists (" + property.Name + ").");

                property.MessageReceived += OnPropertyMessageReceived;

                Properties.Add(property);
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message(e, false));
            }
        }

        /// <inheritdoc />
        public void AddProperty<T>(string name, T value, bool isReadOnly, bool canBeDeleted = true)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new Exception("When adding a property, invalid input was specified!");

                if (!typeof(T).IsSerializable)
                    throw new Exception("The specified property does not support serialization " + "(" + typeof(T) +
                                        ").");

                foreach (var p in Properties)
                    if (p.Name == name)
                        throw new Exception("A property with the same name already exists (" + name + ").");

                var property = new Property<T>(name, value, isReadOnly, canBeDeleted);

                property.MessageReceived += OnPropertyMessageReceived;

                Properties.Add(property);
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message(e, false));
            }
        }

        /// <inheritdoc />
        public object GetPropertyValue(string name)
        {
            try
            {
                foreach (var property in Properties)
                {
                    if (property.Name != name) continue;
                    return property.GetValue();
                }

                return null;
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message(e, false));

                return null;
            }
        }

        /// <inheritdoc />
        public void SetPropertyValue(string name, object value)
        {
            try
            {
                foreach (var property in Properties)
                {
                    if (property.IsReadOnly) continue;
                    if (property.Name != name) continue;

                    property.SetValue(value);
                    return;
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message(e, false));
            }
        }

        /// <inheritdoc />
        public void RemoveProperty(string name)
        {
            try
            {
                foreach (var property in Properties)
                {
                    if (property.Name != name) continue;
                    property.MessageReceived -= OnPropertyMessageReceived;
                    Properties.Remove(property);
                    return;
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new Message(e, false));
            }
        }

        /// <inheritdoc />
        public void RewriteConfiguration(IConfiguration configuration)
        {
            Properties.Clear();

            Name = configuration.Name;

            foreach (var property in configuration.GetProperties())
                Properties.Add(property);
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

            configuration.Initialize();

            return configuration;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var configuration = obj as Configuration;
            if (configuration == null) return false;

            var hash1 = GetHashCode();
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
        ///     Compares two configurations.
        /// </summary>
        /// <param name="other">Other configuration.</param>
        /// <returns>Whether two configurations are equals.</returns>
        protected bool Equals(Configuration other)
        {
            return Id.Equals(other.Id) && Name == other.Name && Equals(Properties, other.Properties);
        }

        /// <summary>
        ///     Notifies when property's message resecived.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Arguments.</param>
        private void OnPropertyMessageReceived(object sender, IMessage e)
        {
            OnMessageReceived(sender, e);
        }
    }
}