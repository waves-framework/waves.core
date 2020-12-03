using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base.Interfaces;

namespace Waves.Core.Base
{
    /// <summary>
    ///     Configuration base class.
    /// </summary>
    [JsonObject]
    public class WavesConfiguration : WavesObject, IWavesConfiguration
    {
        private bool _isDisposed;

        /// <summary>
        ///     Creates new instance of configuration.
        /// </summary>
        public WavesConfiguration()
        {
        }

        /// <summary>
        ///     Creates new instance of configuration.
        /// </summary>
        /// <param name="name">Name.</param>
        public WavesConfiguration(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     Creates new instance of configuration.
        /// </summary>
        /// <param name="id">Id.</param>
        private WavesConfiguration(Guid id)
        {
            Id = id;
        }

        /// <inheritdoc />
        public bool IsInitialized { get; private set; }

        /// <inheritdoc />
        [JsonProperty]
        public override Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc />
        [Reactive]
        [JsonProperty]
        public sealed override string Name { get; set; }
        
        /// <summary>
        ///     Gets collection of properties.
        /// </summary>
        [Reactive]
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
        public ICollection<IWavesProperty> Properties { get; private set; } = new List<IWavesProperty>();

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
                foreach (var property in Properties) 
                    property.MessageReceived += OnPropertyMessageReceived;

                IsInitialized = true;
            }
            catch (Exception e)
            {
                OnMessageReceived(this,new WavesMessage(e, false));

                IsInitialized = false;
            }
        }

        /// <inheritdoc />
        public ICollection<IWavesProperty> GetProperties()
        {
            return Properties;
        }

        /// <inheritdoc />
        public void AddProperty(IWavesProperty property)
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
                OnMessageReceived(this,new WavesMessage(e, false));
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

                var property = new WavesProperty<T>(name, value, isReadOnly, canBeDeleted);

                property.MessageReceived += OnPropertyMessageReceived;

                Properties.Add(property);
            }
            catch (Exception e)
            {
                OnMessageReceived(this,new WavesMessage(e, false));
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
                OnMessageReceived(this,new WavesMessage(e, false));

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
                    if (property.Name != name) continue;

                    property.SetValue(value);
                    return;
                }
            }
            catch (Exception e)
            {
                OnMessageReceived(this, new WavesMessage(e, false));
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
                OnMessageReceived(this, new WavesMessage(e, false));
            }
        }

        /// <inheritdoc />
        public void RewriteConfiguration(IWavesConfiguration configuration)
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
            var configuration = new WavesConfiguration(Id);

            foreach (var property in Properties)
                configuration.Properties.Add((IWavesProperty) property.Clone());

            configuration.Initialize();

            return configuration;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var configuration = obj as WavesConfiguration;
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
        protected bool Equals(WavesConfiguration other)
        {
            return Id.Equals(other.Id) && Name == other.Name && Equals(Properties, other.Properties);
        }

        /// <summary>
        ///     Notifies when property's message resecived.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Arguments.</param>
        private void OnPropertyMessageReceived(object sender, IWavesMessage e)
        {
            OnMessageReceived(this,e);
        }
    }
}